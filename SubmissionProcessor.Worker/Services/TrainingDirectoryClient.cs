using System.Text.Json;
using SubmissionProcessor.Worker.Models;
using SubmissionProcessor.Worker.Resources;
namespace SubmissionProcessor.Worker.Services
{
    public class TrainingDirectoryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TrainingDirectoryClient> _logger;

        public TrainingDirectoryClient(IHttpClientFactory httpClientFactory, ILogger<TrainingDirectoryClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DirectoryProfile?> GetTraineeProfileAsync(int traineeId, string? correlationId, CancellationToken cancellationToken)
        {
            HttpClient client =_httpClientFactory.CreateClient(StringConstants.DirectoryApiClientName);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/directory/trainees/{traineeId}/profile");

            if (!string.IsNullOrEmpty(correlationId))
            {
                request.Headers.Add(StringConstants.CorrelationIdHeader, correlationId);
            }

            try
            {
                HttpResponseMessage response = await client.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(StringConstants.DirectoryApiReturnedError, response.StatusCode, traineeId);
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonSerializer.Deserialize<DirectoryProfile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (TaskCanceledException)
            {
                _logger.LogError(StringConstants.DirectoryApiTimeout, traineeId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, StringConstants.DirectoryApiFailed);
                return null;
            }
        }
    }
}