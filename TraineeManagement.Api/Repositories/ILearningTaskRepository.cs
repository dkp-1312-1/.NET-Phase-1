using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface ILearningTaskRepository
    {
        Task<(List<LearningTask> learningTasks, int totalRecords)> GetLearningTasksAsync(SearchDTO<LTStatusType> searchDTO);
        Task<LearningTask> GetByIdAsync(int id);
        Task AddAsync(LearningTask learningTask);
        Task UpdateAsync(LearningTask learningTask);
        Task DeleteAsync(LearningTask learningTask);
    }
}
