using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository) 
        { 
            _reviewRepository = reviewRepository; 
        }

        public async Task<PagedResponseDTO<ReviewResponseDTO>> GetAll(SearchDTO<RSType> search)
        {
            (List<Review>? reviews, int totalRecords) = await _reviewRepository.GetReviewsAsync(search);

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
            Review review = await _reviewRepository.GetByIdAsync(id);
            return review != null ? MapToResponse(review) : null!;
        }

        public async Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request)
        {
            Review newReview = new Review(request);

            await _reviewRepository.AddAsync(newReview);
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
