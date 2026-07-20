using TraineeManagement.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Api.Services;
using Microsoft.Extensions.Localization;
using TraineeManagement.Api.Resources;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Api.Controllers
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

            LearningTaskResponseDTO task = await _learningTaskService.GetById(Id);
            if (task == null)
            {
                throw new NotFoundException(StringConstants.TaskNotFound(Id));
            }
            ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = task, Success = true };

            return Ok(task);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateLearningTask([FromBody] CreateLearningTaskRequestDTO request)
        {

            LearningTaskResponseDTO newTask = await _learningTaskService.Create(request);
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
            LearningTaskResponseDTO updatedTask = await _learningTaskService.Update(Id, request);
            if (updatedTask == null)
            {
                throw new NotFoundException(StringConstants.TaskNotFound(Id));
            }
            ApiResponseDTO<LearningTaskResponseDTO> result = new ApiResponseDTO<LearningTaskResponseDTO> { Data = updatedTask, Success = true };
            _logger.LogInformation("Task updated successfully with ID {Id}", updatedTask.Id);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteLearningTask(int Id)
        {
            bool isDeleted = await _learningTaskService.Delete(Id);
            _logger.LogInformation("Task Deleted successfully with ID {Id}", Id);
            return Ok(new{Success=isDeleted});
        }
    }
}
