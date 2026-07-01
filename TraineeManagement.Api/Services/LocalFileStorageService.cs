using TraineeManagement.Api.Utils;
using Org.BouncyCastle.Crypto.Prng;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Repositories;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using TraineeManagement.Api.DTOs;
public class LocalFileStorageService : IFileStorageService
{
    private readonly ISubmissionFileRepository _submissionFileRepository;
    private readonly IPublishRabbitMQService _publishRabbitMQService;
    private readonly IProcessingJobRepository _processingJobRepository;
    private readonly string _storageRoot;

    public LocalFileStorageService(ISubmissionFileRepository submissionFileRepository, IPublishRabbitMQService publishRabbitMQService,IProcessingJobRepository processingJobRepository)
    {
        _storageRoot = Config.StorageRoot;
        _submissionFileRepository = submissionFileRepository;
        _publishRabbitMQService = publishRabbitMQService;
        _processingJobRepository=processingJobRepository;
    }
    public async Task<SubmissionProcessingRequestedDTO> SaveAsync(FileUploadRequestDTO request)
    {
        string extension = Path.GetExtension(request.File.FileName).ToLower();

        using Stream content = request.File.OpenReadStream();
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
            SubmissionId = request.SubmissionId,
            OriginalFileName = request.File.FileName,
            StorageFileName = storageName,
            ContentType = request.File.ContentType,
            SizeBytes = request.File.Length,
            UploadedDate = DateTime.UtcNow,
            CheckSum = checksum,
            UploadedByUserId = request.UserId
        };
        await _submissionFileRepository.AddAsync(metadata);
        await _submissionFileRepository.SaveChangesAsync();


        SubmissionProcessingRequestedDTO message = new SubmissionProcessingRequestedDTO
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = request.CorrelationId,
            SubmissionId = request.SubmissionId,
            FileId = metadata.Id,
            RequestedAt = DateTime.UtcNow
        };
        ProcessingJob job =new ProcessingJob(message);
        await _processingJobRepository.AddAsync(job);

        _publishRabbitMQService.PublishSubmission(message);
        return message;
    }

    public Task<Stream> OpenReadAsync(string storageName)
    {
        string fullPath = Path.Combine(_storageRoot!=""?_storageRoot:"/app/uploads", storageName);
        if (!File.Exists(fullPath))
        {
            throw new NotFoundException(StringConstants.fileNotFound);
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
        Console.WriteLine(fullPath);
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
