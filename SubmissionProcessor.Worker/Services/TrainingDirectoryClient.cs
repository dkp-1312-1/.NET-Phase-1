using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using SubmissionProcessor.Worker.Models;
namespace SubmissionProcessor.Worker.Services
{
    public class TrainingDirectoryClient
    {
        private readonly ILogger<TrainingDirectoryClient> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public TrainingDirectoryClient(IHttpClientFactory httpClientFactory, ILogger<TrainingDirectoryClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DirectoryProfile?> GetTraineeProfileAsync(int traineeId, string? correlationId, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/directory/trainees/{traineeId}/profile");

            if (!string.IsNullOrEmpty(correlationId))
            {
                request.Headers.Add("X-Correlation-ID", correlationId);
            }

            try
            {
                using HttpClient client = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Directory API returned {StatusCode} for Trainee {Id}", response.StatusCode, traineeId);
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonSerializer.Deserialize<DirectoryProfile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Directory API request timed out for Trainee {Id}", traineeId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call Directory API.");
                return null;
            }
        }
    }
}