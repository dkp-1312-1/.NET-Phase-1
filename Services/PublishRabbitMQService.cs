using System.ComponentModel;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using TraineeManagement.Api.DTOs;
using RabbitMQ.Client;

namespace TraineeManagement.Api.Services
{
    public class PublishRabbitMQService : IPublishRabbitMQService
    {
        private readonly ILogger<PublishRabbitMQService> _logger;
        public PublishRabbitMQService(ILogger<PublishRabbitMQService> logger)
        {
            _logger = logger;
        }
        public async void PublishSubmission(SubmissionProcessingRequestedDTO message)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = Config.RabbitHostName ?? "Hostname",
                Port = Config.RabbitPort,
                UserName = Config.RabbitUserName,
                Password = Config.RabbitPassword,
                VirtualHost = Config.RabbitVirtualHost
            };

            await using IConnection connection = await factory.CreateConnectionAsync();
            await using IChannel channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "submission-processing",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            string json = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(json);
            var properties = new BasicProperties
            {
                Persistent = true
            };
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "submission-processing",
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published Message {MessageId} for Submission {SubmissionId}. Correlation: {CorrelationId}", message.MessageId, message.SubmissionId, message.CorrelationId);
        }
    }
}
