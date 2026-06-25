using Microsoft.AspNetCore.Mvc;
namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
            status = "running",
            application = "Trainee Management",
            timestamp = DateTime.UtcNow
            });
        }
    }
}
