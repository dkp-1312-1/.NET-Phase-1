using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.Resources;
namespace TraineeManagement.Api.Controllers
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
            [FromQuery] SearchDTO<MentorStatusType> searchObject)
        {
            PagedResponseDTO<MentorResponseDTO> mentors = await _mentorServices.GetAll(searchObject);
            return Ok(mentors);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMentorById(int Id)
        {
            MentorResponseDTO mentor = await _mentorServices.GetById(Id);
            if (mentor == null)
            {
                throw new NotFoundException(StringConstants.MentorNotFound(Id));
            }
            ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = mentor, Success = true };
            return Ok(result);
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateMentor([FromBody] CreateMentorRequestDTO newMentor)
        {

            MentorResponseDTO mentor = await _mentorServices.Create(newMentor);
            ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = mentor, Success = true };
            _logger.LogInformation("Mentor created successfully with ID {Id}", mentor.Id);
            return CreatedAtAction(
                nameof(GetMentorById), new { Id = mentor.Id },
               result);
        }

        [HttpPut("{Id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateMentor(
            int Id, [FromBody] UpdateMentorRequestDTO mentor)
        {
            MentorResponseDTO updatedMentor = await _mentorServices.Update(Id, mentor);
            if (updatedMentor == null)
            {
                throw new NotFoundException(StringConstants.MentorNotFound(Id));
            }
            ApiResponseDTO<MentorResponseDTO> result = new ApiResponseDTO<MentorResponseDTO> { Data = updatedMentor, Success = true };
            _logger.LogInformation("Mentor updated successfully with ID {Id}", updatedMentor.Id);
            return Ok(result);

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMentor(int Id)
        {
            bool isDeleted = await _mentorServices.Delete(Id);
            _logger.LogInformation("Mentor Deleted successfully with ID {Id}", Id);
            return Ok(new{Success=isDeleted});
        }
    }
}
