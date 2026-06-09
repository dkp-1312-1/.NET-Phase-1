using TraineeManagement1.DTOs;
using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Models;
using TraineeManagement1.Data;
namespace TraineeManagement1.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly AppDbContext _context;
        public TraineeService(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<TraineeResponseDTO>> GetAll(string? name=null,string? email=null,string? techstack=null,string? status=null)
        {
            var trainees=await _context.Trainees.ToListAsync();
            if(name!=null||email!=null||techstack!=null||status!=null)
            {
                trainees=await _context.Trainees.Where( t=>
                (!string.IsNullOrWhiteSpace(name)&&t.FirstName.ToLower().Contains(name))|| (!string.IsNullOrWhiteSpace(name)&&t.LastName.ToLower().Contains(name))||(!string.IsNullOrWhiteSpace(email)&&t.Email.ToLower().Contains(email))||(!string.IsNullOrWhiteSpace(techstack)&&t.TechStack.ToLower().Contains(techstack))||(!string.IsNullOrWhiteSpace(status)&&t.Status.ToLower().Contains(status))).ToListAsync();
                return trainees.Select(MapToResponse);
            }
            return trainees.Select(MapToResponse);
        }
        public async Task<TraineeResponseDTO> GetById(int id)
        {
            var trainee =await _context.Trainees.FindAsync(id);
            return trainee != null ? MapToResponse(trainee) : null;
        }
        public async Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO trainee)
        {
            var newTrainee = new Trainee
            {
                id=_context.Trainees.Any()? _context.Trainees.Max(t=>t.id)+1:1,
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
        public async Task<TraineeResponseDTO> Update(int id,UpdateTraineeRequestDTO trainee)
        {
            var updatedTrainee = await _context.Trainees.FindAsync(id);
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
        public async Task<bool> Delete(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
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
                id=trainee.id,
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
