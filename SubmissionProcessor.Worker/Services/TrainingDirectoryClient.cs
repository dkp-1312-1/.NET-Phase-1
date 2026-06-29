using System.Text.Json;
using SubmissionProcessor.Worker.Models;
namespace SubmissionProcessor.Worker.Services;

public class DirectoryProfile
{
    public int TraineeId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string MentorName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class TrainingDirectoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TrainingDirectoryClient> _logger;

    public TrainingDirectoryClient(HttpClient httpClient, ILogger<TrainingDirectoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DirectoryProfile?> GetTraineeProfileAsync(int traineeId, string? correlationId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/directory/trainees/{traineeId}/profile");
        
        if (!string.IsNullOrEmpty(correlationId))
        {
            request.Headers.Add("X-Correlation-ID", correlationId);
        }

        try
        {
            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            
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