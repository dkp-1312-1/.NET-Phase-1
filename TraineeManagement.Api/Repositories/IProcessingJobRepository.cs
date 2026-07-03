using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface IProcessingJobRepository
    {
        Task<ProcessingJob> GetByIdAsync(string id);
        Task AddAsync(ProcessingJob job);
    }
}