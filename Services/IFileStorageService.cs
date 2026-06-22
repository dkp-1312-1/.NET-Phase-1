namespace TraineeManagement.Api.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(Stream content, string extension);
        Task<Stream> OpenReadAsync(string storageName);
        Task<bool> ExistsAsync(string storageName);
        Task DeleteAsync(string storageName);
    }
}