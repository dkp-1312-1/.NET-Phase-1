using TraineeManagement.Api.Utils;
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
            
            (List<TaskAssignment>? assignments, int totalRecords) = await _taskAssignmentRepository.GetTaskAssignmentsAsync(search);
            
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
            TaskAssignmentResponseDTO cachedTask = await _cacheService.GetAsync<TaskAssignmentResponseDTO>(cacheKey);
            if (cachedTask != null)
            {
                return cachedTask;
            }
            TaskAssignment? assignment = await _taskAssignmentRepository.GetByIdAsync(id);
            if(assignment!=null)
                await _cacheService.SetAsync(cacheKey, MapToResponse(assignment));
            return assignment != null ? MapToResponse(assignment) : null;
        }

        public async Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request)
        {
            if (request.DueDate < DateTime.UtcNow)
                throw new BadRequestException(StringConstants.dueDateError);
            TaskAssignment newAssignment = new TaskAssignment(request);

            await _taskAssignmentRepository.AddAsync(newAssignment);
            return MapToResponse(newAssignment);
        }

        public async Task<TaskAssignmentResponseDTO> UpdateStatus(int id, TAType status)
        {
            string cacheKeyId = StringConstants.taskAssignment(id);
            await _cacheService.RemoveAsync(cacheKeyId);
            TaskAssignment assignment = await _taskAssignmentRepository.GetByIdAsync(id);
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
