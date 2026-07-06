using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface IReviewRepository
    {
        Task<(List<Review> reviews, int totalRecords)> GetReviewsAsync(SearchDTO<RSType> searchDTO);
        Task<Review> GetByIdAsync(int id);
        Task AddAsync(Review review);
    }
}
