using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface IProcessingJobRepository
    {
        Task<ProcessingJob> GetByIdAsync(string id);
        Task AddAsync(ProcessingJob job);
    }
}
