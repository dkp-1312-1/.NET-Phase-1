using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class LearningTaskRepository : ILearningTaskRepository
    {
        private readonly AppDbContext _context;

        public LearningTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<LearningTask> learningTasks, int totalRecords)> GetLearningTasksAsync(SearchDTO<LTStatusType> searchDTO)
        {
            IQueryable<LearningTask> Query = _context.LearningTasks.AsQueryable();
            
            if (searchDTO.Name != null)
            {
                string Name = searchDTO.Name.ToLower();
                Query = Query.Where(t =>
                    t.Title.ToLower().Contains(Name) || 
                    t.Description.ToLower().Contains(Name) || 
                    t.ExpectedTechStack.ToLower().Contains(Name));
            }
            
            if (searchDTO.Status != null)
            {
                LTStatusType Status = searchDTO.Status;
                Query = Query.Where(t => t.Status == Status);
            }

            int totalRecords = await Query.CountAsync();
            List<LearningTask> learningTasks = await Query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize).Take(searchDTO.PageSize).ToListAsync();
            
            return (learningTasks, totalRecords);
        }

        public async Task<LearningTask> GetByIdAsync(int id)
        {
            return await _context.LearningTasks.FindAsync(id);
        }

        public async Task AddAsync(LearningTask learningTask)
        {
            await _context.LearningTasks.AddAsync(learningTask);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LearningTask learningTask)
        {
            _context.LearningTasks.Update(learningTask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LearningTask learningTask)
        {
            _context.LearningTasks.Remove(learningTask);
            await _context.SaveChangesAsync();
        }
    }
}
