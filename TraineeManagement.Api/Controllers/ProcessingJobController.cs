using TraineeManagement.Api.Utils;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Resources;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProcessingJobController:ControllerBase
    {
        private readonly IProcessingJobService _processingJobService;
        public ProcessingJobController(IProcessingJobService processingJobService)
        {
            _processingJobService=processingJobService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcessingJob(int id)
        {
            ProcessingJobResponseDTO job =await _processingJobService.GetById(id);
            if(job==null)
            {
                throw new NotFoundException(StringConstants.JobNotFound(id));
            }
            return Ok(job);
        }

    }
}
