using TraineeManagement.Api.Utils;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;
using TraineeManagement.Api.Resources;

namespace TraineeManagement.Api.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ICacheService _cacheService;

        public SubmissionService(ISubmissionRepository submissionRepository,ICacheService cacheService)
        {
            _submissionRepository = submissionRepository;
            _cacheService=cacheService;
        }

        public async Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO<SubType> search)
        {
            (List<Submission>? submissions, int totalRecords) = await _submissionRepository.GetSubmissionsAsync(search);
            
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
            string cacheKey = StringConstants.submission(id);
            SubmissionResponseDTO cachedSubmission = await _cacheService.GetAsync<SubmissionResponseDTO>(cacheKey);
            if (cachedSubmission != null)
            {
                return cachedSubmission;
            }
            Submission? sub = await _submissionRepository.GetByIdAsync(id);
            if(sub!=null)
                await _cacheService.SetAsync(cacheKey, MapToResponse(sub));
            return sub != null ? MapToResponse(sub) : null;
        }

        public async Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request)
        {
            bool subExists = await _submissionRepository.HasSubmissionForTaskAsync(request.TaskAssignmentId);

            if (subExists)
                request.Status = SubType.Resubmitted;

            Submission newSub = new Submission(request);
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
