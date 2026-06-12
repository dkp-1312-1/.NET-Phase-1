using TraineeManagement1.DTOs;
using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Models;
using TraineeManagement1.Data;
using System.Collections.Generic;
using System.Linq;
namespace TraineeManagement1.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly AppDbContext _context;
        public TraineeService(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<TraineeResponseDTO>> GetAll(SearchTraineeDTO trainee)
        {
            IQueryable<Trainee> Query = _context.Trainees.AsQueryable();
            if(trainee.Name!=null)
            {
                var Name=trainee.Name.ToLower();
                Query=Query.Where( t=>
                t.FirstName.ToLower().Contains(Name)|| t.LastName.ToLower().Contains(Name)||t.Email.ToLower().Contains(Name)||t.TechStack.ToLower().Contains(Name)||t.Status.ToLower().Contains(Name));
            }
            var trainees = await Query.ToListAsync();
            return trainees.Select(MapToResponse);
        }
        public async Task<TraineeResponseDTO> GetById(int Id)
        {
            var trainee =await _context.Trainees.FindAsync(Id);
            return trainee != null ? MapToResponse(trainee) : null;
        }
        public async Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO trainee)
        {
            var newTrainee = new Trainee
            {
                Id=_context.Trainees.Any()? _context.Trainees.Max(t=>t.Id)+1:1,
                FirstName = trainee.FirstName,
                LastName = trainee.LastName,
                Email = trainee.Email,
                TechStack = trainee.TechStack,
                Status = trainee.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await _context.Trainees.AddAsync(newTrainee);
            await _context.SaveChangesAsync();
            return MapToResponse(newTrainee);
        }
        public async Task<TraineeResponseDTO> Update(int Id,UpdateTraineeRequestDTO trainee)
        {
            var updatedTrainee = await _context.Trainees.FindAsync(Id);
            if (updatedTrainee == null)
                return null;
            updatedTrainee.FirstName = trainee.FirstName;
            updatedTrainee.LastName = trainee.LastName;
            updatedTrainee.Email = trainee.Email;
            updatedTrainee.TechStack = trainee.TechStack;
            updatedTrainee.Status = trainee.Status;
            updatedTrainee.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapToResponse(updatedTrainee);
        }
        public async Task<bool> Delete(int Id)
        {
            var trainee = await _context.Trainees.FindAsync(Id);
            if (trainee == null)
                return false;
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
            return true;
        }
        private TraineeResponseDTO MapToResponse(Trainee trainee)
        {
            return new TraineeResponseDTO
            {
                Id=trainee.Id,
                FirstName = trainee.FirstName,
                LastName = trainee.LastName,
                Email = trainee.Email,
                TechStack = trainee.TechStack,
                Status = trainee.Status,
                CreatedDate = trainee.CreatedDate,
                UpdatedDate = trainee.UpdatedDate
            };
        }
    }
}
