using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Review> reviews, int totalRecords)> GetReviewsAsync(SearchDTO<RSType> searchDTO)
        {
            var query = _context.Reviews.AsQueryable();
            var totalRecords = await query.CountAsync();
            var reviews = await query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize)
                                     .Take(searchDTO.PageSize).ToListAsync();
            
            return (reviews, totalRecords);
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
    }
}
