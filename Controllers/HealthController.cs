using Microsoft.AspNetCore.Mvc;
namespace TraineeManagement1.Controllers1
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
