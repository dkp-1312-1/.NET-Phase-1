using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
 
namespace TraineeManagement1.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
 
        public ReviewService(AppDbContext context) { _context = context; }
 
        public async Task<PagedResponseDTO<ReviewResponseDTO>> GetAll(SearchDTO search)
        {
            var query = _context.Reviews.AsQueryable();
            var totalRecords = await query.CountAsync();
            var reviews = await query.Skip((search.PageNumber - 1) * search.PageSize)
                                     .Take(search.PageSize).ToListAsync();
 
            return new PagedResponseDTO<ReviewResponseDTO>
            {
                PageNumber = search.PageNumber,
                PageSize = search.PageSize,
                TotalRecords = totalRecords, 
                Data = reviews.Select(MapToResponse)
            };
        }
 
        public async Task<ReviewResponseDTO> GetById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            return review != null ? MapToResponse(review) : null;
        }
 
        public async Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request)
        {
            var newReview = new Review
            {
                Id = _context.Reviews.Any() ? _context.Reviews.Max(t => t.Id) + 1 : 1,
                SubmissionId = request.SubmissionId, MentorId = request.MentorId,
                Feedback = request.Feedback, Score = request.Score,
                ReviewStatus = request.ReviewStatus, ReviewedDate = DateTime.UtcNow
            };
 
            await _context.Reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return MapToResponse(newReview);
        }
 
        private ReviewResponseDTO MapToResponse(Review review)
        {
            return new ReviewResponseDTO
            {
                Id = review.Id, SubmissionId = review.SubmissionId, MentorId = review.MentorId,
                Feedback = review.Feedback, Score = review.Score,
                ReviewStatus = review.ReviewStatus, ReviewedDate = review.ReviewedDate
            };
        }
    }
}