using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
using TraineeManagement1.Services;
namespace TraineeManagement1.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class TraineesController : ControllerBase {
    private readonly ITraineeService _traineeServices;

    public TraineesController(ITraineeService traineeService) {
      _traineeServices = traineeService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTrainees(
        [FromQuery]string TraineeName) {
      try {
        SearchTraineeDTO SearchObject=new SearchTraineeDTO{ Name =TraineeName};
       
        var trainees = await _traineeServices.GetAll(SearchObject);
        ApiResponsesDTO result =
            new ApiResponsesDTO{ Data = new DataObject{ Trainees = trainees, Total = trainees.Count() },
                  Success = true };
        return Ok(result);
      } catch (Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end.");
      }
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetTraineeById(int Id) {
      try {
        var trainee = await _traineeServices.GetById(Id);
        ApiResponseDTO result = new ApiResponseDTO{ Data =  trainee , Success = true };
        if (trainee == null) {
          return NotFound(result);
        }
        return Ok(result);
      } catch (Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end.");
      }
    }
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateTrainee(
        [FromBody] CreateTraineeRequestDTO newTrainee) {
      try {
        var trainee = await _traineeServices.Create(newTrainee);
        ApiResponseDTO result = new ApiResponseDTO{ Data =  trainee , Success = true };
        return CreatedAtAction(
            nameof(GetTraineeById), new { Id = trainee.Id },
           result);
      } catch (Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end.");
      }
    }

    [HttpPut("{Id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateTrainee(
        int Id, [FromBody] UpdateTraineeRequestDTO trainee) {
      try {
        var updatedTrainee = await _traineeServices.Update(Id, trainee);
        ApiResponseDTO result = new ApiResponseDTO{ Data = updatedTrainee , Success = true };
        if (updatedTrainee == null) {
          return NotFound(result);
        }
        return Ok(result);
      } catch (Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end.");
      }
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteTrainee(int Id) {
      try {
        var isDeleted = await _traineeServices.Delete(Id);
        if (!isDeleted)
          return NotFound();
        return Ok(isDeleted);
      } catch (Exception ex) {
        return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end.");
      }
    }
  }
}
