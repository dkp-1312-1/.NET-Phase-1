using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Repositories;
namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubmissionFilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ISubmissionFileRepository _fileRepository;
        private readonly ILogger<SubmissionFilesController> _logger;

        public SubmissionFilesController(
            IFileStorageService fileStorageService,
            ISubmissionFileRepository fileRepository,
            ILogger<SubmissionFilesController> logger)
        {
            _fileStorageService = fileStorageService;
            _fileRepository = fileRepository;
            _logger = logger;
        }
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileRecord = await _fileRepository.GetByIdAsync(id);
            if (fileRecord == null) return NotFound("File not found.");

            var currentUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (int.TryParse(currentUserIdStr, out int currentUserId))
            {
                if (fileRecord.UploadedByUserId != currentUserId && currentUserRole != "Admin" && currentUserRole != "Mentor")
                {
                    return Forbid();
                }
            }

            var fileExists = await _fileStorageService.ExistsAsync(fileRecord.StorageFileName);
            if (!fileExists) return NotFound("The physical file could not be found.");

            var stream = await _fileStorageService.OpenReadAsync(fileRecord.StorageFileName);
            return File(stream, fileRecord.ContentType, fileRecord.OriginalFileName);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var fileRecord = await _fileRepository.GetByIdAsync(id);
            if (fileRecord == null) return NotFound("File not found.");

            var currentUserIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (int.TryParse(currentUserIdStr, out int currentUserId))
            {
                if (fileRecord.UploadedByUserId != currentUserId && currentUserRole != "Admin")
                {
                    return Forbid();
                }
            }
            
            try
            {
                await _fileStorageService.DeleteAsync(fileRecord.StorageFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete physical file.");
                return StatusCode(500, "Error deleting the file from storage.");
            }

            await _fileRepository.DeleteAsync(fileRecord);
            await _fileRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}