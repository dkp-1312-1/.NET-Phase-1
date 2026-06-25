using Org.BouncyCastle.Crypto.Prng;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Repositories;
using TraineeManagement.Api.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using TraineeManagement.Api.DTOs;
public class LocalFileStorageService : IFileStorageService
{
    private readonly ISubmissionFileRepository _submissionFileRepository;
    private readonly IPublishRabbitMQService _publishRabbitMQService;
    private readonly string _storageRoot;

    public LocalFileStorageService(ISubmissionFileRepository submissionFileRepository,IPublishRabbitMQService publishRabbitMQService)
    {
        _storageRoot = Config.StorageRoot;
        _submissionFileRepository = submissionFileRepository;
        _publishRabbitMQService=publishRabbitMQService;
    }
    public async Task<SubmissionProcessingRequestedDTO> SaveAsync(Stream content, string extension, int submissionId, IFormFile file, int userId)
    {
        string checksum;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = await sha256.ComputeHashAsync(content);
            checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        content.Position = 0;
        string storageName = $"{Guid.NewGuid()}{extension}";
        string fullPath = Path.Combine(_storageRoot, storageName);

        using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        await content.CopyToAsync(fileStream);

        SubmissionFile metadata = new SubmissionFile
        {
            SubmissionId = submissionId,
            OriginalFileName = file.FileName,
            StorageFileName = storageName,
            ContentType = file.ContentType,
            SizeBytes = file.Length,
            UploadedDate = DateTime.UtcNow,
            CheckSum = checksum,
            UploadedByUserId = userId
        };
        await _submissionFileRepository.AddAsync(metadata);
        await _submissionFileRepository.SaveChangesAsync();

        string correlationId = Guid.NewGuid().ToString();
        string messageId = Guid.NewGuid().ToString();

        SubmissionProcessingRequestedDTO message = new SubmissionProcessingRequestedDTO
        {
            MessageId = messageId,
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = metadata.Id,
            RequestedAt = DateTime.UtcNow
        };
        _publishRabbitMQService.PublishSubmission(message);

        return message;
    }

    public Task<Stream> OpenReadAsync(string storageName)
    {
        string fullPath = Path.Combine(_storageRoot, storageName);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File with storage name {storageName} not found.");
        }
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }
    public Task<bool> ExistsAsync(string storageName)
    {
        string fullPath = Path.Combine(_storageRoot, storageName);
        return Task.FromResult(File.Exists(fullPath));
    }
    public async Task<bool> DeleteAsync(SubmissionFile file)
    {
        string fullPath = Path.Combine(_storageRoot, file.StorageFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        await _submissionFileRepository.DeleteAsync(file);
        await _submissionFileRepository.SaveChangesAsync();
        return true;
    }

    public async Task<SubmissionFile> FindRecord(int id)
    {
        SubmissionFile file = await _submissionFileRepository.GetByIdAsync(id);
        return file;
    }
    private SubmissionFileResponseDTO MaptoResponse(SubmissionFile file)
    {
        return new SubmissionFileResponseDTO
        {
            Id = file.Id,
            SubmissionId = file.SubmissionId,
            OriginalFileName = file.OriginalFileName,
            StorageFileName = file.StorageFileName,
            ContentType = file.ContentType,
            SizeBytes = file.SizeBytes,
            UploadedDate = file.UploadedDate,
            CheckSum = file.CheckSum,
            UploadedByUserId = file.UploadedByUserId
        };
    }
}