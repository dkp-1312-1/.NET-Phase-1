using TraineeManagement.Data.Models;
namespace TraineeManagement.Api.Repositories
{
    public interface ISubmissionFileRepository
    {
        Task<SubmissionFile> GetByIdAsync(int id);
        Task AddAsync(SubmissionFile file);
        Task DeleteAsync(SubmissionFile file);
        Task SaveChangesAsync();

    }
}
