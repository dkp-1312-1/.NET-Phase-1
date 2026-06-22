using System.Runtime.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Services;
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
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            
        }
    }
}