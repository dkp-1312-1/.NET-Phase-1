using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Enums;
using Microsoft.EntityFrameworkCore;
using SubmissionProcessor.Worker.Services;
using SubmissionProcessor.Worker.Models;
using SubmissionProcessor.Worker.Resources;
using SubmissionProcessor.Worker.Utils;

namespace SubmissionProcessor.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    private ConnectionFactory _factory;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = Config.RabbitHostName,
            Port = Config.RabbitPort,
            UserName = Config.RabbitUserName,
            Password = Config.RabbitPassword,
            VirtualHost = Config.RabbitVirtualHost
        };
        _factory = factory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        int maxRetries = 10;
        int currentAttempt = 0;
        TimeSpan delay = TimeSpan.FromSeconds(5);

        while (currentAttempt < maxRetries)
        {
            try
            {
                currentAttempt++;
                _logger.LogInformation(StringConstants.AttemptingConnect, currentAttempt, maxRetries);

                _connection = await _factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

                _logger.LogInformation(StringConstants.SuccessfullyConnected);
                break; 
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, StringConstants.FailedConnect, currentAttempt);

                if (currentAttempt >= maxRetries)
                {
                    _logger.LogError(StringConstants.ExhaustedAttempts, maxRetries);
                    throw; 
                }

                _logger.LogInformation(StringConstants.WaitingRetry, delay.TotalSeconds);
                await Task.Delay(delay, cancellationToken);
            }
        }
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel == null) return;

        await _channel.ExchangeDeclareAsync(
            exchange: StringConstants.DeadLetterExchange,
            type: "direct",
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
        await _channel.QueueDeclareAsync(
            queue: StringConstants.SubmissionFailedQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
        await _channel.QueueBindAsync(
            queue: StringConstants.SubmissionFailedQueue,
            exchange: StringConstants.DeadLetterExchange,
            routingKey: StringConstants.SubmissionFailedRoutingKey,
            arguments: null,
            cancellationToken: stoppingToken
        );

        Dictionary<string, object?> queueArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", StringConstants.DeadLetterExchange },
            { "x-dead-letter-routing-key", StringConstants.SubmissionFailedRoutingKey }
        };
        await _channel.QueueDeclareAsync(
            queue: StringConstants.SubmissionProcessingQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs,
            cancellationToken: stoppingToken
        );
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
        _logger.LogInformation(StringConstants.GoingToProcessQueue);
        consumer.ReceivedAsync += ProcessMessageAsync;

        await _channel.BasicConsumeAsync(
            queue: StringConstants.SubmissionProcessingQueue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
    private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
    {
        if (_channel == null) return;
        byte[] body = ea.Body.ToArray();
        string messageJson = Encoding.UTF8.GetString(body);
        SubmissionProcessingRequestedDTO? request = JsonSerializer.Deserialize<SubmissionProcessingRequestedDTO>(messageJson);
        if (request == null)
        {
            _logger.LogError(StringConstants.RequestNotFound);
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
            return;
        }
        using (_logger.BeginScope("=>=>CorrelationId<=<= {CorrelationId}", request.CorrelationId))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            TrainingDirectoryClient directoryClient = scope.ServiceProvider.GetRequiredService<TrainingDirectoryClient>();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            ProcessingJob? job = await dbContext.ProcessingJobs.FirstOrDefaultAsync(p => p.MessageId == request.MessageId);
            if (job == null)
            {
                _logger.LogWarning(StringConstants.JobNotFound, request.MessageId);
                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                return;
            }
            if (job.Status == ProcessingJobType.Completed || job.Status == ProcessingJobType.Failed)
            {
                _logger.LogInformation(StringConstants.JobAlreadyStatus, job.Id, job.Status);
                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                return;
            }
            try
            {
                job.Status = ProcessingJobType.Processing;
                job.StartedAt = DateTime.UtcNow;
                job.Attempts += 1;
                await dbContext.SaveChangesAsync();
                _logger.LogInformation(StringConstants.ProcessingJob, job.Id, job.Attempts);

                DirectoryProfile? traineeProfile = await directoryClient.GetTraineeProfileAsync(request.SubmissionId, request.CorrelationId, CancellationToken.None);

                if (traineeProfile != null)
                {
                    _logger.LogInformation(StringConstants.SuccessfullyRetrievedProfile, traineeProfile);
                }
                else
                {
                    _logger.LogWarning(StringConstants.FallbackActivated);
                }

                SubmissionFile? fileRecord = await dbContext.SubmissionFiles.FindAsync(request.FileId);
                if (fileRecord == null)
                {
                    _logger.LogError(StringConstants.FileRecordNotFound);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    return;
                }
                string uploadsDir = "/app/uploads";
                string fullPath = Path.Combine(uploadsDir, fileRecord.StorageFileName!);
                if (!File.Exists(fullPath))
                {
                    _logger.LogError("File not found in storage: {FullPath}", fullPath);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    return;
                }
                await using Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using SHA256 sha256 = SHA256.Create();
                byte[] hashBytes = await sha256.ComputeHashAsync(stream);
                string checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                if (checksum != fileRecord.CheckSum)
                {
                    _logger.LogError(StringConstants.ChecksumNotMatched);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    return;
                }
                else
                {
                    _logger.LogInformation(StringConstants.ChecksumMatched);
                }
                job.Status = ProcessingJobType.Completed;
                job.CompletedAt = DateTime.UtcNow;
                Submission? submission = await dbContext.Submissions.FindAsync(request.SubmissionId);
                if (submission != null)
                {
                    TaskAssignment? taskAssignment = await dbContext.TaskAssignments.FindAsync(submission.TaskAssignmentId);
                    if (taskAssignment != null)
                    {
                        taskAssignment.Status = TAType.Submitted;
                    }
                }
                await dbContext.SaveChangesAsync();

                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                _logger.LogInformation(StringConstants.JobSuccessfullyCompleted, job.Id, checksum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StringConstants.ErrorProcessingJob, job.Id);
                if (job.Attempts > 3)
                {
                    job.Status = ProcessingJobType.Failed;
                    job.ErrorSummary = ex.Message;
                    job.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();

                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    _logger.LogError(StringConstants.JobExhaustedRetries, job.Id);
                }
                else
                {
                    job.Status = ProcessingJobType.Queued;
                    await dbContext.SaveChangesAsync();
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            }
        }
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }
        if (_connection is not null)
        {
            await _connection.CloseAsync(cancellationToken);
            await _connection.DisposeAsync();
        }
        await base.StopAsync(cancellationToken);
    }
}