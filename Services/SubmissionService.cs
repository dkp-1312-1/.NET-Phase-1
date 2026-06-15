using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;

namespace TraineeManagement1.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly AppDbContext _context;

        public SubmissionService(AppDbContext context) { _context = context; }

        public async Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO search)
        {
            var query = _context.Submissions.AsQueryable();
            var totalRecords = await query.CountAsync();
            var submissions = await query.Skip((search.PageNumber - 1) * search.PageSize)
                                         .Take(search.PageSize).ToListAsync();

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
            var sub = await _context.Submissions.FindAsync(id);
            return sub != null ? MapToResponse(sub) : null;
        }

        public async Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request)
        {
            var newSub = new Submission
            {
                Id = _context.Submissions.Any() ? _context.Submissions.Max(t => t.Id) + 1 : 1,
                TaskAssignmentId = request.TaskAssignmentId,
                SubmissionUrl = request.SubmissionUrl,
                Notes = request.Notes,
                SubmittedDate = DateTime.UtcNow,
                Status = request.Status
            };

            await _context.Submissions.AddAsync(newSub);
            await _context.SaveChangesAsync();
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