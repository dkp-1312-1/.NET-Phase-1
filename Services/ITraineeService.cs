using System.Diagnostics.Eventing.Reader;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
using TraineeManagement1.Models;
namespace TraineeManagement1.Services
{
    public interface ITraineeService
    {
        Task<PagedResponseDTO<TraineeResponseDTO>> GetAll(SearchDTO<TraineeStatusType> trainee);
        Task<TraineeResponseDTO>? GetById(int Id);
        Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO Trainee);
        Task<TraineeResponseDTO> Update(int Id, UpdateTraineeRequestDTO request);
        Task<bool> Delete(int Id);
    }
}
