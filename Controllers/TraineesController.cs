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
  public class TraineesController : ControllerBase
  {

    private readonly ITraineeService _traineeServices;
    private readonly ILogger<TraineesController> _logger;

    public TraineesController(ITraineeService traineeService,ILogger<TraineesController> logger)
    {
      _traineeServices = traineeService;
       _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrainees(
        [FromQuery] SearchDTO searchObject)
    {
      try
      {
        PagedResponseDTO<TraineeResponseDTO> trainees = await _traineeServices.GetAll(searchObject);
        return Ok(trainees);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
      }
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetTraineeById(int Id)
    {
      try
      {
        var trainee = await _traineeServices.GetById(Id);
        ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = trainee, Success = true };
        if (trainee == null)
        {
           _logger.LogWarning("Trainee with ID {Id} was not found.", Id);
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
    public async Task<IActionResult> CreateTrainee(
        [FromBody] CreateTraineeRequestDTO newTrainee)
    {
      try
      {
        var trainee = await _traineeServices.Create(newTrainee);
        ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = trainee, Success = true };
        _logger.LogInformation("Trainee created successfully with ID {Id}", trainee.Id);
        return CreatedAtAction(
            nameof(GetTraineeById), new { Id = trainee.Id },
           result);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
      }
    }

    [HttpPut("{Id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateTrainee(
        int Id, [FromBody] UpdateTraineeRequestDTO trainee)
    {
      try
      {
        var updatedTrainee = await _traineeServices.Update(Id, trainee);
        ApiResponseDTO<TraineeResponseDTO> result = new ApiResponseDTO<TraineeResponseDTO> { Data = updatedTrainee, Success = true };
        if (updatedTrainee == null)
        {
           _logger.LogWarning("Trainee with ID {Id} was not found.", Id);
          return NotFound(result);
        }
        _logger.LogInformation("Trainee updated successfully with ID {Id}", updatedTrainee.Id);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
      }
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteTrainee(int Id)
    {
      try
      {
        var isDeleted = await _traineeServices.Delete(Id);
        if (!isDeleted)
        {
           _logger.LogWarning("Trainee with ID {Id} was not found.", Id);
          return NotFound();
        }
        _logger.LogInformation("Trainee Deleted successfully with ID {Id}", Id);
        return Ok(isDeleted);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
      }
    }
  }
}
