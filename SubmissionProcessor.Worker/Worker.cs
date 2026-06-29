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

namespace SubmissionProcessor.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = "localhost",
            VirtualHost = "mqhost"
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);


        await _channel.ExchangeDeclareAsync(
            exchange: "dead-letter-exchange",
            type: "direct",
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
        await _channel.QueueDeclareAsync(
            queue: "submission-failed",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        );
        await _channel.QueueBindAsync(
            queue: "submission-failed",
            exchange: "dead-letter-exchange",
            routingKey: "submission-failed",
            arguments: null,
            cancellationToken: stoppingToken
        );

        Dictionary<string, object?> queueArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "dead-letter-exchange" },
            { "x-dead-letter-routing-key", "submission-failed" }
        };
        await _channel.QueueDeclareAsync(
            queue: "submission-processing",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs,
            cancellationToken: stoppingToken
        );
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += ProcessMessageAsync;

        await _channel.BasicConsumeAsync(
            queue: "submission-processing",
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
        Task.Delay(TimeSpan.FromSeconds(5));
        byte[] body = ea.Body.ToArray();
        string messageJson = Encoding.UTF8.GetString(body);
        SubmissionProcessingRequestedDTO? request = JsonSerializer.Deserialize<SubmissionProcessingRequestedDTO>(messageJson);
        if (request == null)
        {
            _logger.LogError("Request wasnot found.");
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
            return;
        }
        using (_logger.BeginScope("=>=>CorrelationId<=<= {CorrelationId}",request.CorrelationId))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            TrainingDirectoryClient directoryClient = scope.ServiceProvider.GetRequiredService<TrainingDirectoryClient>();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            IFileStorageService fileStorage = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            ProcessingJob? job = await dbContext.ProcessingJobs.FirstOrDefaultAsync(p => p.MessageId == request.MessageId);
            if (job == null)
            {
                _logger.LogWarning("Job {MessageId} not found.", request.MessageId);
                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                return;
            }
            if (job.Status == ProcessingJobType.Completed || job.Status == ProcessingJobType.Failed)
            {
                _logger.LogInformation("Job {Id} already {Status}. Skipping...", job.Id, job.Status);
                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                return;
            }
            try
            {
                job.Status = ProcessingJobType.Processing;
                job.StartedAt = DateTime.UtcNow;
                job.Attempts += 1;
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Processing Job {Id}. Attempt {Attempt}", job.Id, job.Attempts);

                DirectoryProfile? traineeProfile = await directoryClient.GetTraineeProfileAsync(request.SubmissionId, request.CorrelationId, CancellationToken.None);

                if (traineeProfile != null)
                {
                    _logger.LogInformation("Successfully retrieved profile for processing: {Profile}", traineeProfile);
                }
                else
                {
                    _logger.LogWarning("Fallback activated: Processing file without Trainee Profile data.");
                }

                SubmissionFile? fileRecord = await dbContext.SubmissionFiles.FindAsync(request.FileId);
                await using Stream stream = await fileStorage.OpenReadAsync(fileRecord.StorageFileName);
                using SHA256 sha256 = SHA256.Create();
                byte[] hashBytes = await sha256.ComputeHashAsync(stream);
                string checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                if(checksum!=fileRecord.CheckSum)
                {
                    _logger.LogError("Checksum not matched: Cannot Process File.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    return;
                }
                else
                {
                    _logger.LogInformation("Checksum matched: Process File.");
                }
                job.Status = ProcessingJobType.Completed;
                job.CompletedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();

                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
                _logger.LogInformation("Job {Id} successfully completed with Checksum: {Checksum}", job.Id, checksum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Job {Id}");
                if (job.Attempts > 3)
                {
                    job.Status = ProcessingJobType.Failed;
                    job.ErrorSummary = ex.Message;
                    job.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();

                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                    _logger.LogError("Job {Id} exhausted retries.Moved to Dead Letter Queue.", job.Id);
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