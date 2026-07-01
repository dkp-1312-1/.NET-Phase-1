using System.ComponentModel;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using TraineeManagement.Api.DTOs;
using RabbitMQ.Client;

namespace TraineeManagement.Api.Utils
{
    public class PublishRabbitMQService : IPublishRabbitMQService
    {
        private readonly ILogger<PublishRabbitMQService> _logger;
        private readonly IConnectionFactory _factory;
        public PublishRabbitMQService(ILogger<PublishRabbitMQService> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory
            {
                HostName = Config.RabbitHostName ?? "Hostname",
                Port = Config.RabbitPort,
                UserName = Config.RabbitUserName,
                Password = Config.RabbitPassword,
                VirtualHost = Config.RabbitVirtualHost
            };
        }
        public async void PublishSubmission(SubmissionProcessingRequestedDTO message)
        {
            await using IConnection connection = await _factory.CreateConnectionAsync();
            await using IChannel channel = await connection.CreateChannelAsync();
            Dictionary<string, object?> queueArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "dead-letter-exchange" },
            { "x-dead-letter-routing-key", "submission-failed" }
        };
            await channel.QueueDeclareAsync(queue: "submission-processing",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: queueArgs
                );
            string json = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(json);
            BasicProperties properties = new BasicProperties
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
