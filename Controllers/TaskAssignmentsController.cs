using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement1.DTOs;
using TraineeManagement1.Services;
 
namespace TraineeManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskAssignmentsController : ControllerBase
    {
        private readonly ITaskAssignmentService _service;
        private readonly ILogger<TaskAssignmentsController> _logger;
 
        public TaskAssignmentsController(ITaskAssignmentService service, ILogger<TaskAssignmentsController> logger)
        {
            _service = service;
            _logger = logger;
        }
 
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchDTO search)
        {
            try { return Ok(await _service.GetAll(search)); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null) { _logger.LogWarning("Assignment {Id} not found.", id); return NotFound(); }
                return Ok(new ApiResponseDTO<TaskAssignmentResponseDTO> { Data = result, Success = true });
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskAssignmentRequestDTO request)
        {
            try
            {
                var result = await _service.Create(request);
                _logger.LogInformation("Assignment created: {Id}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponseDTO<TaskAssignmentResponseDTO> { Data = result, Success = true });
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); } // Catch validation logic
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
 
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAssignmentStatusRequestDTO request)
        {
            try
            {
                var result = await _service.UpdateStatus(id, request.Status);
                if (result == null) return NotFound();
                return Ok(new ApiResponseDTO<TaskAssignmentResponseDTO> { Data = result, Success = true });
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
 