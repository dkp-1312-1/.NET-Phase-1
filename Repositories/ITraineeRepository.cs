using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface ITraineeRepository
    {
        Task<(List<Trainee> trainees, int totalRecords)> GetTraineesAsync(SearchDTO<TraineeStatusType> searchDTO);
        Task<Trainee> GetByIdAsync(int id);
        Task AddAsync(Trainee trainee);
        Task UpdateAsync(Trainee trainee);
        Task DeleteAsync(Trainee trainee);
    }
}
