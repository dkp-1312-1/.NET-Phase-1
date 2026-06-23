using System.Runtime.CompilerServices;
using System.Security.Permissions;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.Api.Repositories
{
    public class SubmissionFileRepository : ISubmissionFileRepository
    {
        private readonly AppDbContext _context;
        public SubmissionFileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<SubmissionFile> GetByIdAsync(int id)
        {
            return await _context.SubmissionFiles.FindAsync(id);
        }
        public async Task AddAsync(SubmissionFile file)
        {
            await _context.SubmissionFiles.AddAsync(file);
        }
        public Task DeleteAsync(SubmissionFile file)
        {
            _context.SubmissionFiles.Remove(file);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetNextIdAsync()
        {
            return await _context.SubmissionFiles.AnyAsync() ? await _context.SubmissionFiles.MaxAsync(t => t.Id) + 1 : 1;
        }
    }
}