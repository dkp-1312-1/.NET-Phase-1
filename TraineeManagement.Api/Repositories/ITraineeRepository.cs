using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

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
