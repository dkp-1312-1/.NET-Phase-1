using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
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
            string cacheKey = StringConstants.submission(StringConstants.all);
            var submissions = await _cacheService.GetAsync<List<Submission>>(cacheKey);
            var totalRecords = submissions?.Count()??0;
            if (submissions == null)
            {
                (submissions, totalRecords) = await _submissionRepository.GetSubmissionsAsync(search);
                await _cacheService.SetAsync(cacheKey, submissions, TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));
            }
            
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
            var cachedSubmission = await _cacheService.GetAsync<SubmissionResponseDTO>(cacheKey);
            if (cachedSubmission != null)
            {
                return cachedSubmission;
            }
            var sub = await _submissionRepository.GetByIdAsync(id);
            await _cacheService.SetAsync(cacheKey, MapToResponse(sub), TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));
            return sub != null ? MapToResponse(sub) : null;
        }

        public async Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request)
        {
            string cacheKey = StringConstants.submission(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
            var subExists = await _submissionRepository.HasSubmissionForTaskAsync(request.TaskAssignmentId);

            if (subExists)
                request.Status = SubType.Resubmitted;

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