using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface ISubmissionRepository
    {
        Task<(List<Submission> submissions, int totalRecords)> GetSubmissionsAsync(SearchDTO<SubType> searchDTO);
        Task<Submission> GetByIdAsync(int id);
        Task<bool> HasSubmissionForTaskAsync(int taskAssignmentId);
        Task AddAsync(Submission submission);
    }
}
