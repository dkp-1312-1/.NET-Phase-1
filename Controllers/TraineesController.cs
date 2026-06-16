using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
using TraineeManagement1.Services;
using TraineeManagement1.Middleware;
using Microsoft.AspNetCore.Authorization;
namespace TraineeManagement1.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class TraineesController : ControllerBase
  {

    private readonly ITraineeService _traineeServices;
    private readonly ILogger<TraineesController> _logger;

    public TraineesController(ITraineeService traineeService, ILogger<TraineesController> logger)
    {
      _traineeServices = traineeService;
      _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrainees(
        [FromQuery] SearchDTO searchObject)
    {
      PagedResponseDTO<TraineeResponseDTO> trainees = await _traineeServices.GetAll(searchObject);
      return Ok(trainees);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetTraineeById(int Id)
    {
      var trainee = await _traineeServices.GetById(Id);
      ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = trainee, Success = true };
      if (trainee == null)
      {
        throw new NotFoundException($"Trainee with id {Id} isnot found");
      }
      return Ok(result);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateTrainee(
        [FromBody] CreateTraineeRequestDTO newTrainee)
    {
      var trainee = await _traineeServices.Create(newTrainee);
      ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = trainee, Success = true };
      _logger.LogInformation("Trainee created successfully with ID {Id}", trainee.Id);
      return CreatedAtAction(
          nameof(GetTraineeById), new { Id = trainee.Id },
         result);
    }


    [HttpPut("{Id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateTrainee(
        int Id, [FromBody] UpdateTraineeRequestDTO trainee)
    {
      var updatedTrainee = await _traineeServices.Update(Id, trainee);
      ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = updatedTrainee, Success = true };
      if (updatedTrainee == null)
      {
        throw new NotFoundException($"Trainee with id {Id} isnot found");
      }
      _logger.LogInformation("Trainee updated successfully with ID {Id}", updatedTrainee.Id);
      return Ok(result);

    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteTrainee(int Id)
    {
      var isDeleted = await _traineeServices.Delete(Id);
      if (!isDeleted)
      {
        throw new NotFoundException($"Trainee with id {Id} isnot found");
      }
      _logger.LogInformation("Trainee Deleted successfully with ID {Id}", Id);
      return Ok(isDeleted);
    }
  }
}

