using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _context;

        public SubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Submission> submissions, int totalRecords)> GetSubmissionsAsync(SearchDTO<SubType> searchDTO)
        {
            var query = _context.Submissions.AsQueryable();
            var totalRecords = await query.CountAsync();
            var submissions = await query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize)
                                         .Take(searchDTO.PageSize).ToListAsync();
            
            return (submissions, totalRecords);
        }

        public async Task<Submission> GetByIdAsync(int id)
        {
            return await _context.Submissions.FindAsync(id);
        }

        public async Task<bool> HasSubmissionForTaskAsync(int taskAssignmentId)
        {
            return await _context.Submissions.AnyAsync(t => t.TaskAssignmentId == taskAssignmentId);
        }

        public async Task<int> GetNextIdAsync()
        {
            return await _context.Submissions.AnyAsync() ? await _context.Submissions.MaxAsync(t => t.Id) + 1 : 1;
        }

        public async Task AddAsync(Submission submission)
        {
            await _context.Submissions.AddAsync(submission);
            await _context.SaveChangesAsync();
        }
    }
}
