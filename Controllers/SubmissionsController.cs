using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement1.DTOs;
using TraineeManagement1.Services;
using Microsoft.Extensions.Localization;
using TraineeManagement1.Resources;
namespace TraineeManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _service;
        private readonly ILogger<SubmissionController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SubmissionController(ISubmissionService service, ILogger<SubmissionController> logger,IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _logger = logger;
            _localizer=localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchDTO search)
        {
            return Ok(await _service.GetAll(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
            {
                throw new NotFoundException(_localizer["SubmissionNotFound", id]);
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
