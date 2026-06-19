using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Services;
using Microsoft.Extensions.Localization;
using TraineeManagement.Api.Resources;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _service;
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(ISubmissionService service, ILogger<SubmissionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchDTO<SubType> search)
        {
            return Ok(await _service.GetAll(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
            {
                throw new NotFoundException(StringConstants.SubmissionNotFound(id));
            }
            return Ok(new ApiResponseDTO<SubmissionResponseDTO>
            {
                Data = result,
                Success = true
            });

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubmissionRequestDTO request)
        {
            var result = await _service.Create(request);
            _logger.LogInformation("Assignment created: {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponseDTO<SubmissionResponseDTO> { Data = result, Success = true });

        }
    }
}
