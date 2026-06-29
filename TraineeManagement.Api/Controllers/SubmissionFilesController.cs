using TraineeManagement.Api.Utils;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Resources;
using System.Security.Claims;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.DTOs;
using System.Data;
using TraineeManagement.Api.Models;
namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionFilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<SubmissionFilesController> _logger;

        public SubmissionFilesController(
            IFileStorageService fileStorageService,
            ILogger<SubmissionFilesController> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        [HttpPost("/api/submissions/{submissionId}/files")]
        public async Task<IActionResult> UploadFile(int submissionId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(StringConstants.fileEmpty);

            if (file.Length > Config.RedisFileSizeLimit * 1024 * 1024)
                throw new PayloadTooLargeException(StringConstants.fileSizeExceed(Config.RedisFileSizeLimit));

            string[] allowExtensions = new[] { ".pdf", ".zip", ".docx" };
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowExtensions.Contains(extension))
                throw new BadRequestException(StringConstants.InvalidFileType);

            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            FileUploadRequestDTO request = new FileUploadRequestDTO
            {
                SubmissionId = submissionId,
                File = file,
                UserId = userId
            };

            SubmissionProcessingRequestedDTO message = await _fileStorageService.SaveAsync(request);

            _logger.LogInformation("File submitted. MessageId={messageId}, CorrelationId={correlationId}, SubmissionId={subId}, FileId={fileId}",
                message.MessageId, message.CorrelationId, submissionId, message.FileId);

            return Accepted(new
            {
                TrackingId = message.MessageId,
                CorrelationId = message.CorrelationId,
                SubmissionId = submissionId,
                FileId = message.FileId,
                Status = SubType.Submitted
            });
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            SubmissionFile fileRecord = await _fileStorageService.FindRecord(id);
            if (fileRecord == null)
                throw new NotFoundException(StringConstants.SubmissionFileNotFound(id));
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.TryParse(userIdString, out int userId))
            {
                if (!(fileRecord.UploadedByUserId == userId || userRole == RoleType.Admin.ToString() || userRole == RoleType.Mentor.ToString()))
                {
                    throw new UnauthorizedException(StringConstants.noAccessDownload);
                }
            }
            bool fileExists = await _fileStorageService.ExistsAsync(fileRecord.StorageFileName);
            if (!fileExists)
                throw new NotFoundException(StringConstants.fileNotFound);
            Stream stream = await _fileStorageService.OpenReadAsync(fileRecord.StorageFileName);
            return File(stream, fileRecord.ContentType, fileRecord.OriginalFileName);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            SubmissionFile fileRecord = await _fileStorageService.FindRecord(id);
            if (fileRecord == null)
                throw new NotFoundException(StringConstants.SubmissionFileNotFound(id));
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.TryParse(userIdString, out int userId))
            {
                if (!(fileRecord.UploadedByUserId == userId || userRole == RoleType.Admin.ToString()))
                {
                    throw new UnauthorizedException(StringConstants.noAccessDownload);
                }
            }
            try
            {
                bool isDeleted = await _fileStorageService.DeleteAsync(fileRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete physical file.");
                throw new Exception(StringConstants.deleteFileError);
            }
            return Ok(new { Success = true });
        }
    }
}
