using TraineeManagement.Data.Models;
using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IFileStorageService
    {
        Task<SubmissionProcessingRequestedDTO> SaveAsync(FileUploadRequestDTO request);
        Task<Stream> OpenReadAsync(string storageName);
        Task<bool> ExistsAsync(string storageName);
        Task<bool> DeleteAsync(SubmissionFile file);
        Task<SubmissionFile> FindRecord(int id);
    }
}
