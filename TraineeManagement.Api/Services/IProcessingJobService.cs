using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IProcessingJobService
    {
        Task<ProcessingJobResponseDTO> GetById(string id);
    }
}
