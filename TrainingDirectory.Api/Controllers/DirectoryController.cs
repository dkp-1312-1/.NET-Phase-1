using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DirectoryController : ControllerBase
{
    private readonly ILogger<DirectoryController> _logger;

    public DirectoryController(ILogger<DirectoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet("trainees/{id}/profile")]
    public IActionResult GetTraineeProfile(int id)
    {
        // Log the Correlation ID so we can trace requests across services!
        var correlationId = Request.Headers["X-Correlation-ID"].FirstOrDefault();
        _logger.LogInformation("Fetching profile for Trainee {Id}. CorrelationId: {CorrelationId}", id, correlationId);

        // Simulate a random failure to test our Circuit Breaker later
        if (new Random().Next(1, 10) <= 2) 
        {
            return StatusCode(500, "Internal Directory Error!");
        }

        return Ok(new 
        { 
            TraineeId = id, 
            Department = "Engineering", 
            MentorName = "Jane Doe",
            IsActive = true
        });
    }
}