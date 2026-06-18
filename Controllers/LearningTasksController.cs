using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement1.DTOs;
using TraineeManagement1.Services;
using Microsoft.Extensions.Localization;
using TraineeManagement1.Resources;
using TraineeManagement1.Models;
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
        public async Task<IActionResult> GetAllLearningTasks([FromQuery] SearchDTO<LTStatusType> searchObject)
        {
            PagedResponseDTO<LearningTaskResponseDTO> tasks = await _learningTaskService.GetAll(searchObject);
            return Ok(tasks);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetLearningTaskById(int Id)
        {

            var task = await _learningTaskService.GetById(Id);
            ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = task, Success = true };
            if (task == null)
            {
                throw new NotFoundException(SharedResource.TaskNotFound(Id));
            }

            return Ok(task);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateLearningTask([FromBody] CreateLearningTaskRequestDTO request)
        {

            var newTask = await _learningTaskService.Create(request);
            ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = newTask, Success = true };
            _logger.LogInformation("Task created successfully with ID {Id}", newTask.Id);
            return CreatedAtAction(
                nameof(GetLearningTaskById), new { Id = newTask.Id },
               result);
        }

        [HttpPut("{Id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLearningTask(int Id, [FromBody] UpdateLearningTaskRequestDTO request)
        {
            var updatedTask = await _learningTaskService.Update(Id, request);
            ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = updatedTask, Success = true };
            if (updatedTask == null)
            {
                throw new NotFoundException(SharedResource.TaskNotFound(Id));
            }
            _logger.LogInformation("Task updated successfully with ID {Id}", updatedTask.Id);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteLearningTask(int Id)
        {
            var isDeleted = await _learningTaskService.Delete(Id);
            if (!isDeleted)
            {
               throw new NotFoundException(SharedResource.TaskNotFound(Id));
            }
            _logger.LogInformation("Task Deleted successfully with ID {Id}", Id);
            return Ok(isDeleted);
        }
    }
}