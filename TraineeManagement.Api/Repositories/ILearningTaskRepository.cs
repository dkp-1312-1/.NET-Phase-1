using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

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