using TraineeManagement.Api.Data;
using TraineeManagement.Api.Models;
namespace TraineeManagement.Api.Repositories
{
    public class ProcessingJobRepository : IProcessingJobRepository
    {
        private readonly AppDbContext _context;

        public ProcessingJobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProcessingJob> GetByIdAsync(int id)
        {
            return await _context.ProcessingJobs.FindAsync(id);
        }
    }
}