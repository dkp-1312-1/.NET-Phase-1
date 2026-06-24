using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.Repositories
{
    public class MentorRepository : IMentorRepository
    {
        private readonly AppDbContext _context;

        public MentorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Mentor> mentors, int totalRecords)> GetMentorsAsync(SearchDTO<MentorStatusType> searchDTO)
        {
            IQueryable<Mentor> Query = _context.Mentors.AsQueryable();
            
            if (searchDTO.Name != null)
            {
                var Name = searchDTO.Name.ToLower();
                Query = Query.Where(t =>
                    t.FirstName.ToLower().Contains(Name) || 
                    t.LastName.ToLower().Contains(Name) || 
                    t.Email.ToLower().Contains(Name) || 
                    t.Expertise.ToLower().Contains(Name));
            }
            
            if (searchDTO.Status != null)
            {
                var Status = searchDTO.Status;
                Query = Query.Where(t => t.Status == Status);
            }
            
            var totalRecords = await Query.CountAsync();
            var mentors = await Query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize).Take(searchDTO.PageSize).ToListAsync();
            
            return (mentors, totalRecords);
        }

        public async Task<Mentor> GetByIdAsync(int id)
        {
            return await _context.Mentors.FindAsync(id);
        }


        public async Task AddAsync(Mentor mentor)
        {
            await _context.Mentors.AddAsync(mentor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mentor mentor)
        {
            _context.Mentors.Update(mentor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Mentor mentor)
        {
            _context.Mentors.Remove(mentor);
            await _context.SaveChangesAsync();
        }
    }
}
