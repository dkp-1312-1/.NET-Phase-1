using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class TraineeRepository : ITraineeRepository
    {
        private readonly AppDbContext _context;

        public TraineeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Trainee> trainees, int totalRecords)> GetTraineesAsync(SearchDTO<TraineeStatusType> searchDTO)
        {
            IQueryable<Trainee> Query = _context.Trainees.AsQueryable();
            
            if (searchDTO.Name != null)
            {
                string Name = searchDTO.Name.ToLower();
                Query = Query.Where(t =>
                    t.FirstName.ToLower().Contains(Name) || 
                    t.LastName.ToLower().Contains(Name) || 
                    t.Email.ToLower().Contains(Name) || 
                    t.TechStack.ToLower().Contains(Name));
            }
            
            if (searchDTO.Status != null)
            {
                TraineeStatusType Status = searchDTO.Status;
                Query = Query.Where(t => t.Status == Status);
            }

            int totalRecords = await Query.CountAsync();
            List<Trainee> trainees = await Query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize).Take(searchDTO.PageSize).ToListAsync();
            
            return (trainees, totalRecords);
        }

        public async Task<Trainee> GetByIdAsync(int id)
        {
            return await _context.Trainees.FindAsync(id);
        }


        public async Task AddAsync(Trainee trainee)
        {
            await _context.Trainees.AddAsync(trainee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trainee trainee)
        {
            _context.Trainees.Update(trainee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Trainee trainee)
        {
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
        }
    }
}
