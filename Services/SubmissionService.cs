using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;

        public SubmissionService(ISubmissionRepository submissionRepository) 
        { 
            _submissionRepository = submissionRepository; 
        }

        public async Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO<SubType> search)
        {
            var (submissions, totalRecords) = await _submissionRepository.GetSubmissionsAsync(search);

            return new PagedResponseDTO<SubmissionResponseDTO>
            {
                PageNumber = search.PageNumber,
                PageSize = search.PageSize,
                TotalRecords = totalRecords,
                Data = submissions.Select(MapToResponse)
            };
        }

        public async Task<SubmissionResponseDTO> GetById(int id)
        {
            var sub = await _submissionRepository.GetByIdAsync(id);
            return sub != null ? MapToResponse(sub) : null;
        }

        public async Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request)
        {
            var subExists = await _submissionRepository.HasSubmissionForTaskAsync(request.TaskAssignmentId);
 
            if (subExists)
                request.Status=SubType.Resubmitted;
 
            var newSub = new Submission
            {
                Id = await _submissionRepository.GetNextIdAsync(),
                TaskAssignmentId = request.TaskAssignmentId,
                SubmissionUrl = request.SubmissionUrl,
                Notes = request.Notes,
                SubmittedDate = DateTime.UtcNow,
                Status = request.Status
            };

            await _submissionRepository.AddAsync(newSub);
            return MapToResponse(newSub);
        }

        private SubmissionResponseDTO MapToResponse(Submission sub)
        {
            return new SubmissionResponseDTO
            {
                Id = sub.Id,
                TaskAssignmentId = sub.TaskAssignmentId,
                SubmissionUrl = sub.SubmissionUrl,
                Notes = sub.Notes,
                SubmittedDate = sub.SubmittedDate,
                Status = sub.Status
            };
        }
    }
}