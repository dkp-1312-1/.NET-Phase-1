using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly AppDbContext _context;

        public TaskAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<TaskAssignment> assignments, int totalRecords)> GetTaskAssignmentsAsync(SearchDTO<TAType> searchDTO)
        {
            IQueryable<TaskAssignment> query = _context.TaskAssignments.AsQueryable();
            
            if (searchDTO.Status != null)
            {
                query = query.Where(t => t.Status == searchDTO.Status);
            }

            int totalRecords = await query.CountAsync();
            List<TaskAssignment> assignments = await query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize)
                                         .Take(searchDTO.PageSize).ToListAsync();
            
            return (assignments, totalRecords);
        }

        public async Task<TaskAssignment> GetByIdAsync(int id)
        {
            return await _context.TaskAssignments.FindAsync(id);
        }

        public async Task AddAsync(TaskAssignment assignment)
        {
            await _context.TaskAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskAssignment assignment)
        {
            _context.TaskAssignments.Update(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
