using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
using TraineeManagement1.Services;
namespace TraineeManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineesController : ControllerBase
    {
        private readonly ITraineeService _traineeServices;
        
        public TraineesController(ITraineeService traineeService)
        {
            _traineeServices = traineeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrainees([FromQuery] string search)
        {
            var trainees = await _traineeServices.GetAll(search);
            return Ok(trainees);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraineeById(int id)
        {
            var trainee = await _traineeServices.GetById(id);
            if(trainee==null)
            {
                return NotFound();
            }
            return Ok(trainee);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrainee([FromBody] CreateTraineeRequestDTO newTrainee)
        {
            var trainee =await _traineeServices.Create(newTrainee);

            return CreatedAtAction(nameof(GetTraineeById), new { id = trainee.id }, trainee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainee(int id, [FromBody] UpdateTraineeRequestDTO trainee)
        {
            var updatedTrainee = await _traineeServices.Update(id, trainee);
            if (updatedTrainee == null)
            {
                return NotFound();
            }
            return Ok(updatedTrainee);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainee(int id)
        {
            var isDeleted =await _traineeServices.Delete(id);
            if (!isDeleted) return NotFound();
            return Ok(isDeleted);
        }

    }
}
