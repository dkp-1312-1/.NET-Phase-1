using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement1.DTOs;
using TraineeManagement1.Services;

namespace TraineeManagement1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LearningTasksController : ControllerBase
    {
        private readonly ILearningTaskService _learningTaskService;
        private readonly ILogger<LearningTasksController> _logger;

        public LearningTasksController(ILearningTaskService learningTaskService, ILogger<LearningTasksController> logger)
        {
            _learningTaskService = learningTaskService;
             _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLearningTasks([FromQuery] SearchDTO searchObject)
        {
            try
            {
                PagedResponseDTO<LearningTaskResponseDTO> tasks = await _learningTaskService.GetAll(searchObject);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetLearningTaskById(int Id)
        {
            try
            {
                var task = await _learningTaskService.GetById(Id);
                ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = task, Success = true };
                if (task == null)
                {
                    _logger.LogWarning("Task with ID {Id} was not found.", Id);
                    return NotFound(result);
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateLearningTask([FromBody] CreateLearningTaskRequestDTO request)
        {
            try
            {
                var newTask =  await _learningTaskService.Create(request);
                ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = newTask, Success = true };
                _logger.LogInformation("Task created successfully with ID {Id}", newTask.Id);
                return CreatedAtAction(
                    nameof(GetLearningTaskById), new { Id = newTask.Id },
                   result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }

        }

        [HttpPut("{Id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLearningTask(int Id, [FromBody] UpdateLearningTaskRequestDTO request)
        {
            
            try
            {
                var updatedTask = await _learningTaskService.Update(Id, request);
                ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = updatedTask, Success = true };
                if (updatedTask == null)
                {
                    _logger.LogWarning("Task with ID {Id} was not found.", Id);
                    return NotFound(result);
                }
                _logger.LogInformation("Task updated successfully with ID {Id}", updatedTask.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteLearningTask(int Id)
        {
            try
            {
                var isDeleted = await _learningTaskService.Delete(Id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Task with ID {Id} was not found.", Id);
                    return NotFound();
                }
                _logger.LogInformation("Task Deleted successfully with ID {Id}", Id);
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end." + ex.Message);
            }




        }
    }
}