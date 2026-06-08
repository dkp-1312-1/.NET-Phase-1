using System.Diagnostics.Eventing.Reader;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
namespace TraineeManagement1.Services
{
    public interface ITraineeService
    {
        Task<IEnumerable<TraineeResponseDTO>> GetAll(string search=null);
        Task<TraineeResponseDTO>? GetById(int id);
        Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO Trainee);
        Task<TraineeResponseDTO> Update(int id, UpdateTraineeRequestDTO request);
        Task<bool> Delete(int id);
    }
}
