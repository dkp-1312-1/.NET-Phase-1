using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface ITaskAssignmentRepository
    {
        Task<(List<TaskAssignment> assignments, int totalRecords)> GetTaskAssignmentsAsync(SearchDTO<TAType> searchDTO);
        Task<TaskAssignment> GetByIdAsync(int id);
        Task AddAsync(TaskAssignment assignment);
        Task UpdateAsync(TaskAssignment assignment);
    }
}
