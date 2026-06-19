using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
 
namespace TraineeManagement.Api.Services
{
    public class LearningTaskService : ILearningTaskService
    {
        private readonly AppDbContext _context;
 
        public LearningTaskService(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<PagedResponseDTO<LearningTaskResponseDTO>> GetAll(SearchDTO<LTStatusType> searchDTO)
        {
            IQueryable<LearningTask> Query = _context.LearningTasks.AsQueryable();
            if (searchDTO.Name != null)
            {
                var Name = searchDTO.Name.ToLower();
                Query = Query.Where(t =>
                t.Title.ToLower().Contains(Name) || t.Description.ToLower().Contains(Name) || t.ExpectedTechStack.ToLower().Contains(Name));
            }
            if (searchDTO.Status != null)
            {
                var Status = searchDTO.Status;
                Query = Query.Where(t =>
               t.Status == Status);
            }
            var totalRecords = await Query.CountAsync();
            var learningTasks = await Query.Skip((searchDTO.PageNumber - 1) * searchDTO.PageSize).Take(searchDTO.PageSize).ToListAsync();
            return new PagedResponseDTO<LearningTaskResponseDTO>
            {
                PageNumber = searchDTO.PageNumber,
                PageSize = searchDTO.PageSize,
                TotalRecords = totalRecords,
                Data = learningTasks.Select(MapToResponse)
            };
        }
 
        public async Task<LearningTaskResponseDTO> GetById(int id)
        {
            var task = await _context.LearningTasks.FindAsync(id);
            return task != null ? MapToResponse(task) : null;
        }
 
        public async Task<LearningTaskResponseDTO> Create(CreateLearningTaskRequestDTO request)
        {
            var newTask = new LearningTask
            {
                Id = _context.LearningTasks.Any() ? _context.LearningTasks.Max(t => t.Id) + 1 : 1,
                Title = request.Title,
                Description = request.Description,
                ExpectedTechStack = request.ExpectedTechStack,
                DueDate = request.DueDate,
                Status = request.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
 
            await _context.LearningTasks.AddAsync(newTask);
            await _context.SaveChangesAsync();
 
            return MapToResponse(newTask);
        }
 
        public async Task<LearningTaskResponseDTO> Update(int id, UpdateLearningTaskRequestDTO request)
        {
            var task = await _context.LearningTasks.FindAsync(id);
            if (task == null) return null;
 
            task.Title = request.Title;
            task.Description = request.Description;
            task.ExpectedTechStack = request.ExpectedTechStack;
            task.DueDate = request.DueDate;
            task.Status = request.Status;
            task.UpdatedDate = DateTime.UtcNow;
 
            await _context.SaveChangesAsync();
            return MapToResponse(task);
        }
 
        public async Task<bool> Delete(int Id)
        {
            var task = await _context.LearningTasks.FindAsync(Id);
            if (task == null) return false;
 
            _context.LearningTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
 
        private LearningTaskResponseDTO MapToResponse(LearningTask task)
        {
            return new LearningTaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ExpectedTechStack = task.ExpectedTechStack,
                DueDate = task.DueDate,
                Status = task.Status,
                CreatedDate = task.CreatedDate,
                UpdatedDate = task.UpdatedDate
            };
        }
    }
}
 