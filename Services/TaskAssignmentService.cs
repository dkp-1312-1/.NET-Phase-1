using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public TaskAssignmentService(ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public async Task<PagedResponseDTO<TaskAssignmentResponseDTO>> GetAll(SearchDTO<TAType> search)
        {
            var (assignments, totalRecords) = await _taskAssignmentRepository.GetTaskAssignmentsAsync(search);

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
            var assignment = await _taskAssignmentRepository.GetByIdAsync(id);
            return assignment != null ? MapToResponse(assignment) : null;
        }

        public async Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request)
        {
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