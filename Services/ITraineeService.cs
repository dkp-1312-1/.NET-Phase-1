using System.Diagnostics.Eventing.Reader;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
namespace TraineeManagement1.Services
{
    public interface ITraineeService
    {
        Task<IEnumerable<TraineeResponseDTO>> GetAll(SearchTraineeDTO trainee);
        Task<TraineeResponseDTO>? GetById(int Id);
        Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO Trainee);
        Task<TraineeResponseDTO> Update(int Id, UpdateTraineeRequestDTO request);
        Task<bool> Delete(int Id);
    }
}
