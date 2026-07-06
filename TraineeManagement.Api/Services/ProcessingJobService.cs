using TraineeManagement.Api.Repositories;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
namespace TraineeManagement.Api.Services
{
    public class ProcessingJobService : IProcessingJobService
    {
        private readonly IProcessingJobRepository _processingJobRepository;
        public ProcessingJobService(IProcessingJobRepository processingJobRepository)
        {
            _processingJobRepository = processingJobRepository;
        }
        public async Task<ProcessingJobResponseDTO> GetById(string id)
        {
            ProcessingJob processingJob = await _processingJobRepository.GetByIdAsync(id);
            return processingJob != null ? MapToResponse(processingJob) : null;
        }
        private ProcessingJobResponseDTO MapToResponse(ProcessingJob job)
        {
            return new ProcessingJobResponseDTO
            {
                Id = job.Id,
                MessageId=job.MessageId,
                CorrelationId = job.CorrelationId,
                SubmissionId = job.SubmissionId,
                Status = job.Status,
                Attempts = job.Attempts,
                ErrorSummary = job.ErrorSummary,
                StartedAt = job.StartedAt,
                CompletedAt = job.CompletedAt
            };
        }
    }
}
