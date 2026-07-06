using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

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
            IQueryable<Review> query = _context.Reviews.AsQueryable();
            int totalRecords = await query.CountAsync();
            List<Review> reviews = await query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize)
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
