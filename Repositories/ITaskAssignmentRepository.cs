using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

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
