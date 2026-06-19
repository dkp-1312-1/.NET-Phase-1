using TraineeManagement.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Data;
using System.Collections.Generic;
using System.Linq;
namespace TraineeManagement.Api.Services
{
    public class MentorService : IMentorService
    {
        private readonly AppDbContext _context;
        public MentorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResponseDTO<MentorResponseDTO>> GetAll(SearchDTO<MentorStatusType> mentor)
        {
            IQueryable<Mentor> Query = _context.Mentors.AsQueryable();
            if (mentor.Name != null)
            {
                var Name = mentor.Name.ToLower();
                Query = Query.Where(t =>
                t.FirstName.ToLower().Contains(Name) || t.LastName.ToLower().Contains(Name) || t.Email.ToLower().Contains(Name) || t.Expertise.ToLower().Contains(Name));
            }
            if (mentor.Status != null)
            {
                MentorStatusType Status = mentor.Status;
                Query = Query.Where(t =>
               t.Status == Status);
            }
            var totalRecords = await Query.CountAsync();
            var mentors = await Query.Skip((mentor.PageNumber - 1) * mentor.PageSize).Take(mentor.PageSize).ToListAsync();
            return new PagedResponseDTO<MentorResponseDTO>
            {
                PageNumber = mentor.PageNumber,
                PageSize = mentor.PageSize,
                TotalRecords = totalRecords,
                Data = mentors.Select(MapToResponse)
            };
        }
        public async Task<MentorResponseDTO> GetById(int Id)
        {
            var mentor = await _context.Mentors.FindAsync(Id);
            return mentor != null ? MapToResponse(mentor) : null;
        }
        public async Task<MentorResponseDTO> Create(CreateMentorRequestDTO mentor)
        {
            var newMentor = new Mentor
            {
                Id = _context.Mentors.Any() ? _context.Mentors.Max(t => t.Id) + 1 : 1,
                FirstName = mentor.FirstName,
                LastName = mentor.LastName,
                Email = mentor.Email,
                Expertise = mentor.Expertise,
                Status = mentor.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await _context.Mentors.AddAsync(newMentor);
            await _context.SaveChangesAsync();
            return MapToResponse(newMentor);
        }
        public async Task<MentorResponseDTO> Update(int Id, UpdateMentorRequestDTO mentor)
        {
            var updatedMentor = await _context.Mentors.FindAsync(Id);
            if (updatedMentor == null)
                return null;
            updatedMentor.FirstName = mentor.FirstName;
            updatedMentor.LastName = mentor.LastName;
            updatedMentor.Email = mentor.Email;
            updatedMentor.Expertise = mentor.Expertise;
            updatedMentor.Status = mentor.Status;
            updatedMentor.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapToResponse(updatedMentor);
        }
        public async Task<bool> Delete(int Id)
        {
            var mentor = await _context.Mentors.FindAsync(Id);
            if (mentor == null)
                return false;
            _context.Mentors.Remove(mentor);
            await _context.SaveChangesAsync();
            return true;
        }
        private MentorResponseDTO MapToResponse(Mentor mentor)
        {
            return new MentorResponseDTO
            {
                Id = mentor.Id,
                FirstName = mentor.FirstName,
                LastName = mentor.LastName,
                Email = mentor.Email,
                Expertise = mentor.Expertise,
                Status = mentor.Status,
                CreatedDate = mentor.CreatedDate,
                UpdatedDate = mentor.UpdatedDate
            };
        }
    }
}
