using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IProcessingJobService
    {
        Task<ProcessingJobResponseDTO> GetById(int id);
    }
}