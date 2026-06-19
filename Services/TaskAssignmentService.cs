using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
 
namespace TraineeManagement1.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly AppDbContext _context;
 
        public TaskAssignmentService(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<PagedResponseDTO<TaskAssignmentResponseDTO>> GetAll(SearchDTO<TAType> search)
        {
            var query = _context.TaskAssignments.AsQueryable();
            
            if (search.Status!=null)
            {
                query = query.Where(t => t.Status == search.Status);
            }
 
            var totalRecords = await query.CountAsync();
            var assignments = await query.Skip((search.PageNumber - 1) * search.PageSize)
                                         .Take(search.PageSize).ToListAsync();
 
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
            var assignment = await _context.TaskAssignments.FindAsync(id);
            return assignment != null ? MapToResponse(assignment) : null;
        }
 
        public async Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request)
        {
            if (request.DueDate < DateTime.UtcNow)
                throw new BadRequestException("DueDate should not be before AssignedDate.");
            
            var newAssignment = new TaskAssignment
            {
                Id = _context.TaskAssignments.Any() ? _context.TaskAssignments.Max(t => t.Id) + 1 : 1,
                TraineeId = request.TraineeId,
                MentorId = request.MentorId,
                LearningTaskId = request.LearningTaskId,
                AssignedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                Status = TAType.Assigned,
                Remarks = request.Remarks
            };
 
            await _context.TaskAssignments.AddAsync(newAssignment);
            await _context.SaveChangesAsync();
            return MapToResponse(newAssignment);
        }
 
        public async Task<TaskAssignmentResponseDTO> UpdateStatus(int id, TAType status)
        {
            var assignment = await _context.TaskAssignments.FindAsync(id);
            if (assignment == null) return null;
 
            assignment.Status = status;
            await _context.SaveChangesAsync();
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