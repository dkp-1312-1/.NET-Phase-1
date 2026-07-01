using TraineeManagement.Api.Utils;
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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService service, ILogger<ReviewController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchDTO<RSType> search)
        {
            return Ok(await _service.GetAll(search));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            ReviewResponseDTO result = await _service.GetById(id);
            if (result == null)
            {
                throw new NotFoundException(StringConstants.ReviewNotFound(id));
            }
            return Ok(new ApiResponseDTO<ReviewResponseDTO>
            {
                Data = result,
                Success = true
            });

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequestDTO request)
        {
            ReviewResponseDTO result = await _service.Create(request);
            _logger.LogInformation("Assignment created: {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponseDTO<ReviewResponseDTO> { Data = result, Success = true });
        }
    }
}

