using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class LearningTaskService : ILearningTaskService
    {
        private readonly ILearningTaskRepository _learningTaskRepository;

        public LearningTaskService(ILearningTaskRepository learningTaskRepository)
        {
            _learningTaskRepository = learningTaskRepository;
        }

        public async Task<PagedResponseDTO<LearningTaskResponseDTO>> GetAll(SearchDTO<LTStatusType> searchDTO)
        {
            var (learningTasks, totalRecords) = await _learningTaskRepository.GetLearningTasksAsync(searchDTO);

            return new PagedResponseDTO<LearningTaskResponseDTO>
            {
                PageNumber = searchDTO.PageNumber,
                PageSize = searchDTO.PageSize,
                TotalRecords = totalRecords,
                Data = learningTasks.Select(MapToResponse)
            };
        }

        public async Task<LearningTaskResponseDTO> GetById(int id)
        {
            var task = await _learningTaskRepository.GetByIdAsync(id);
            return task != null ? MapToResponse(task) : null;
        }

        public async Task<LearningTaskResponseDTO> Create(CreateLearningTaskRequestDTO request)
        {
            var newTask = new LearningTask
            {
                Title = request.Title,
                Description = request.Description,
                ExpectedTechStack = request.ExpectedTechStack,
                DueDate = request.DueDate,
                Status = request.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _learningTaskRepository.AddAsync(newTask);
            return MapToResponse(newTask);
        }

        public async Task<LearningTaskResponseDTO> Update(int id, UpdateLearningTaskRequestDTO request)
        {
            var task = await _learningTaskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.Title = request.Title;
            task.Description = request.Description;
            task.ExpectedTechStack = request.ExpectedTechStack;
            task.DueDate = request.DueDate;
            task.Status = request.Status;
            task.UpdatedDate = DateTime.UtcNow;

            await _learningTaskRepository.UpdateAsync(task);
            return MapToResponse(task);
        }

        public async Task<bool> Delete(int Id)
        {
            var task = await _learningTaskRepository.GetByIdAsync(Id);
            if (task == null) return false;

            await _learningTaskRepository.DeleteAsync(task);
            return true;
        }

        private LearningTaskResponseDTO MapToResponse(LearningTask task)
        {
            return new LearningTaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ExpectedTechStack = task.ExpectedTechStack,
                DueDate = task.DueDate,
                Status = task.Status,
                CreatedDate = task.CreatedDate,
                UpdatedDate = task.UpdatedDate
            };
        }
    }
}
 