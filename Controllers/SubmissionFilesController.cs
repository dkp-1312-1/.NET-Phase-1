using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Resources;
using System.Security.Claims;
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
            {
                throw new BadRequestException(StringConstants.fileEmpty);
            }
            if (file.Length > IntConstants.FileSizeLimit * 1024 * 1024)
            {
                throw new PayloadTooLargeException(StringConstants.fileSizeExceed(IntConstants.FileSizeLimit ));
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowExtensions = new[] { ".pdf", ".zip", "docx" };
            if (!allowExtensions.Contains(extension))
            {
                throw new BadRequestException(StringConstants.InvalidFileType);
            }
            using var stream = file.OpenReadStream();
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdString);
            var metadata = await _fileStorageService.SaveAsync(stream, extension, submissionId, file, userId);
            return Created($"/api/submission-files/{metadata.Id}", metadata);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileRecord = await _fileStorageService.FindRecord(id);
            if (fileRecord == null)
                throw new NotFoundException(StringConstants.SubmissionFileNotFound(id));

            var fileExists = await _fileStorageService.ExistsAsync(fileRecord.StorageFileName);
            if (!fileExists)
                throw new NotFoundException(StringConstants.fileNotFound);
            var stream = await _fileStorageService.OpenReadAsync(fileRecord.StorageFileName);
            return File(stream, fileRecord.ContentType, fileRecord.OriginalFileName);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var fileRecord = await _fileStorageService.FindRecord(id);
            if (fileRecord == null)
                throw new NotFoundException(StringConstants.SubmissionFileNotFound(id));
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.TryParse(userIdString, out int userId))
            {
                if (!(fileRecord.UploadedByUserId == userId || userRole == "Admin"))
                {
                    throw new UnauthorizedException(StringConstants.noAccessDownload);
                }
            }
            try
            {
                var isDeleted= await _fileStorageService.DeleteAsync(fileRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete physical file.");
                throw new Exception(StringConstants.deleteFileError);
            }
            return Ok(new{Success=true});
        }
    }
}