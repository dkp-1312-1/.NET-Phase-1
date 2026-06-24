using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;
using TraineeManagement.Api.Resources;

namespace TraineeManagement.Api.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly ICacheService _cacheService;

        public TaskAssignmentService(ITaskAssignmentRepository taskAssignmentRepository, ICacheService cacheService)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
            _cacheService = cacheService;
        }

        public async Task<PagedResponseDTO<TaskAssignmentResponseDTO>> GetAll(SearchDTO<TAType> search)
        {
            string cacheKey = StringConstants.taskAssignment(StringConstants.all);
            var assignments = await _cacheService.GetAsync<List<TaskAssignment>>(cacheKey);
            var totalRecords = assignments?.Count()??0;
            if (assignments == null)
            {
                (assignments, totalRecords) = await _taskAssignmentRepository.GetTaskAssignmentsAsync(search);
                await _cacheService.SetAsync(cacheKey, assignments, TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));
            }
            
            return new PagedResponseDTO<TaskAssignmentResponseDTO>
            {
                PageNumber = search.PageNumber,
                PageSize = search.PageSize,
                TotalRecords = totalRecords,
                Data = assignments.Select(MapToResponse)
            };
        }

        public async Task<TaskAssignmentResponseDTO> GetById(int id)
        {
            string cacheKey = StringConstants.taskAssignment(id);
            var cachedTask = await _cacheService.GetAsync<TaskAssignmentResponseDTO>(cacheKey);
            if (cachedTask != null)
            {
                return cachedTask;
            }
            var assignment = await _taskAssignmentRepository.GetByIdAsync(id);
            await _cacheService.SetAsync(cacheKey, MapToResponse(assignment), TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));
            return assignment != null ? MapToResponse(assignment) : null;
        }

        public async Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request)
        {
            string cacheKey = StringConstants.taskAssignment(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
            if (request.DueDate < DateTime.UtcNow)
                throw new BadRequestException("DueDate should not be before AssignedDate.");
            var newAssignment = new TaskAssignment
            {
                Id = await _taskAssignmentRepository.GetNextIdAsync(),
                TraineeId = request.TraineeId,
                MentorId = request.MentorId,
                LearningTaskId = request.LearningTaskId,
                AssignedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                Status = TAType.Assigned,
                Remarks = request.Remarks
            };

            await _taskAssignmentRepository.AddAsync(newAssignment);
            return MapToResponse(newAssignment);
        }

        public async Task<TaskAssignmentResponseDTO> UpdateStatus(int id, TAType status)
        {
            string cacheKey = StringConstants.taskAssignment(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
            string cacheKeyId = StringConstants.taskAssignment(id);
            await _cacheService.RemoveAsync(cacheKeyId);
            var assignment = await _taskAssignmentRepository.GetByIdAsync(id);
            if (assignment == null) return null;

            assignment.Status = status;
            await _taskAssignmentRepository.UpdateAsync(assignment);
            return MapToResponse(assignment);
        }

        private TaskAssignmentResponseDTO MapToResponse(TaskAssignment assignment)
        {
            return new TaskAssignmentResponseDTO
            {
                Id = assignment.Id,
                TraineeId = assignment.TraineeId,
                MentorId = assignment.MentorId,
                LearningTaskId = assignment.LearningTaskId,
                AssignedDate = assignment.AssignedDate,
                DueDate = assignment.DueDate,
                Status = assignment.Status,
                Remarks = assignment.Remarks
            };
        }
    }
}