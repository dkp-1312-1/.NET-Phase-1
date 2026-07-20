using TraineeManagement.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Api.Services;
using Microsoft.Extensions.Localization;
using TraineeManagement.Api.Resources;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Api.Controllers
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
        public async Task<IActionResult> GetAll([FromQuery] SearchDTO<TAType> search)
        {
            return Ok(await _service.GetAll(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            TaskAssignmentResponseDTO result = await _service.GetById(id);
            if (result == null)
            {
                 throw new NotFoundException(StringConstants.AssignmentNotFound(id));
            }
            return Ok(new ApiResponseDTO<TaskAssignmentResponseDTO>
            {
                Data = result,
                Success = true
            });

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskAssignmentRequestDTO request)
        {
            TaskAssignmentResponseDTO result = await _service.Create(request);
            _logger.LogInformation("Assignment created: {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponseDTO<TaskAssignmentResponseDTO> { Data = result, Success = true });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAssignmentStatusRequestDTO request)
        {
            TaskAssignmentResponseDTO result = await _service.UpdateStatus(id, request.Status);
            if (result == null)
                throw new NotFoundException(StringConstants.AssignmentNotFound(id));
            return Ok(new ApiResponseDTO<TaskAssignmentResponseDTO> { Data = result, Success = true });
        }
    }
}

