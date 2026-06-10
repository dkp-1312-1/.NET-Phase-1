using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        public async Task<IActionResult> GetAllTrainees([FromQuery] string? name=null,[FromQuery] string? email=null,[FromQuery] string? techstack=null,[FromQuery] string? status=null)
        {
            var trainees = await _traineeServices.GetAll(name!=null?name.ToLower():null,email!=null?email.ToLower():null,techstack!=null?techstack.ToLower():null,status!=null?status.ToLower():null);
            var result=new 
            {
                data=new{Trainees=trainees,total=trainees.Count()} ,
                success=true
            };
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraineeById(int id)
        {
            var trainee = await _traineeServices.GetById(id);
            var result=new 
            {
                data=new{Trainees=trainee} ,
                success=true
                };
            if(trainee==null)
            {
                return NotFound(result);
            }
            
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrainee([FromBody] CreateTraineeRequestDTO newTrainee)
        {
            try
            {
            var trainee =await _traineeServices.Create(newTrainee);
            return CreatedAtAction(nameof(GetTraineeById), new { id = trainee.id }, new 
            {
                data=new{Trainees=trainee} ,
                success=true
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainee(int id, [FromBody] UpdateTraineeRequestDTO trainee)
        {
            var updatedTrainee = await _traineeServices.Update(id, trainee);
            var result=new{
                    data=new{Trainees=updatedTrainee},
                    success=true
                };
            if (updatedTrainee == null)
            {
                return NotFound(result);
            }
            return Ok(result);

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
