using System.Diagnostics.Eventing.Reader;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Services
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
