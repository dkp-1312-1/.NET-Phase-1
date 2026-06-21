using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface ISubmissionRepository
    {
        Task<(List<Submission> submissions, int totalRecords)> GetSubmissionsAsync(SearchDTO<SubType> searchDTO);
        Task<Submission> GetByIdAsync(int id);
        Task<bool> HasSubmissionForTaskAsync(int taskAssignmentId);
        Task<int> GetNextIdAsync();
        Task AddAsync(Submission submission);
    }
}
