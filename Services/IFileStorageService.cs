using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IFileStorageService
    {
        Task<SubmissionProcessingRequestedDTO> SaveAsync(Stream content, string extension,int submissionId,IFormFile file,int userid);
        Task<Stream> OpenReadAsync(string storageName);
        Task<bool> ExistsAsync(string storageName);
        Task<bool> DeleteAsync(SubmissionFile file);
        Task<SubmissionFile> FindRecord(int id);
    }
}