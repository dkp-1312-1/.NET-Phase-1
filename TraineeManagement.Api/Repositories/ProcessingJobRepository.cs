using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.Models;
namespace TraineeManagement.Api.Repositories
{
    public class ProcessingJobRepository : IProcessingJobRepository
    {
        private readonly AppDbContext _context;

        public ProcessingJobRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ProcessingJob job)
        {
            await _context.ProcessingJobs.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task<ProcessingJob> GetByIdAsync(string id)
        {
            return await _context.ProcessingJobs.FirstOrDefaultAsync(pj=>pj.MessageId==id);
        }
    }
}
