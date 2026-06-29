using System.Text.Json;

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

    // The HttpClient is injected automatically by the Factory
    public TrainingDirectoryClient(HttpClient httpClient, ILogger<TrainingDirectoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DirectoryProfile?> GetTraineeProfileAsync(int traineeId, string? correlationId, CancellationToken cancellationToken)
    {
        // 1. Create the request
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/directory/trainees/{traineeId}/profile");
        
        // 2. Propagate the Correlation ID (Task 3.18)
        if (!string.IsNullOrEmpty(correlationId))
        {
            request.Headers.Add("X-Correlation-ID", correlationId);
        }

        try
        {
            // 3. Send the request (Passing the cancellation token is critical!)
            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            // Handle non-success explicitly
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Directory API returned {StatusCode} for Trainee {Id}", response.StatusCode, traineeId);
                return null; // Fallback behavior
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
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