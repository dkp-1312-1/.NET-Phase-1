using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
using TraineeManagement1.Services;
using Microsoft.AspNetCore.Authorization;
namespace TraineeManagement1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MentorsController : ControllerBase
    {
        private readonly IMentorService _mentorServices;
        private readonly ILogger<MentorsController> _logger;

        public MentorsController(IMentorService mentorService, ILogger<MentorsController> logger)
        {
            _mentorServices = mentorService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMentors(
            [FromQuery] SearchDTO searchObject)
        {
            try
            {
                PagedResponseDTO<MentorResponseDTO> mentors = await _mentorServices.GetAll(searchObject);
                return Ok(mentors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMentorById(int Id)
        {
            try
            {
                var mentor = await _mentorServices.GetById(Id);
                ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = mentor, Success = true };
                if (mentor == null)
                {
                    _logger.LogWarning("Mentor with ID {Id} was not found.", Id);
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateMentor([FromBody] CreateMentorRequestDTO newMentor)
        {
            try
            {
                var mentor = await _mentorServices.Create(newMentor);
                ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = mentor, Success = true };
                _logger.LogInformation("Mentor created successfully with ID {Id}", mentor.Id);
                return CreatedAtAction(
                    nameof(GetMentorById), new { Id = mentor.Id },
                   result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }

        [HttpPut("{Id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateMentor(
            int Id, [FromBody] UpdateMentorRequestDTO mentor)
        {
            try
            {
                var updatedMentor = await _mentorServices.Update(Id, mentor);
                ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = updatedMentor, Success = true };
                if (updatedMentor == null)
                {
                    _logger.LogWarning("Mentor with ID {Id} was not found.", Id);
                    return NotFound(result);
                }
                _logger.LogInformation("Mentor updated successfully with ID {Id}", updatedMentor.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMentor(int Id)
        {
            try
            {
                var isDeleted = await _mentorServices.Delete(Id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Mentor with ID {Id} was not found.", Id);
                    return NotFound();
                }
                _logger.LogInformation("Mentor Deleted successfully with ID {Id}", Id);
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }
    }
}
