using Org.BouncyCastle.Crypto.Prng;
using TraineeManagement.Api.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _storageRoot;
    public LocalFileStorageService(IConfiguration configuration)
    {
        _storageRoot = configuration["FileStorage:RootPath"];
        if (!Directory.Exists(_storageRoot))
        {
            Directory.CreateDirectory(_storageRoot);
        }
    }
    public async Task<string> SaveAsync(Stream content, string extension)
    {
        var storageName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_storageRoot, storageName);

        using var fileStream = new FileStream(fullPath, FileMode.Create);
        await content.CopyToAsync(fileStream);
        return storageName;
    }
    public Task<Stream> OpenReadAsync(string storageName)
    {
        var fullPath = Path.Combine(_storageRoot, storageName);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File with storage name {storageName} not found.");
        }
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }
    public Task<bool> ExistsAsync(string storageName)
    {
        var fullPath = Path.Combine(_storageRoot, storageName);
        return Task.FromResult(File.Exists(fullPath));
    }
    public Task DeleteAsync(string storageName)
    {
        var fullPath = Path.Combine(_storageRoot, storageName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
    }
}