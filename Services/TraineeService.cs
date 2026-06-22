using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;

        public TraineeService(ITraineeRepository traineeRepository)
        {
            _traineeRepository = traineeRepository;
        }

        public async Task<PagedResponseDTO<TraineeResponseDTO>> GetAll(SearchDTO<TraineeStatusType> trainee)
        {
            var (trainees, totalRecords) = await _traineeRepository.GetTraineesAsync(trainee);

            return new PagedResponseDTO<TraineeResponseDTO>
            {
                PageNumber = trainee.PageNumber,
                PageSize = trainee.PageSize,
                TotalRecords = totalRecords,
                Data = trainees.Select(MapToResponse)
            };
        }

        public async Task<TraineeResponseDTO> GetById(int Id)
        {
            var trainee = await _traineeRepository.GetByIdAsync(Id);
            return trainee != null ? MapToResponse(trainee) : null;
        }

        public async Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO trainee)
        {
            var newTrainee = new Trainee
            {
                Id = await _traineeRepository.GetNextIdAsync(),
                FirstName = trainee.FirstName,
                LastName = trainee.LastName,
                Email = trainee.Email,
                TechStack = trainee.TechStack,
                Status = trainee.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _traineeRepository.AddAsync(newTrainee);
            return MapToResponse(newTrainee);
        }

        public async Task<TraineeResponseDTO> Update(int Id, UpdateTraineeRequestDTO trainee)
        {
            var updatedTrainee = await _traineeRepository.GetByIdAsync(Id);
            if (updatedTrainee == null)
                return null;

            updatedTrainee.FirstName = trainee.FirstName;
            updatedTrainee.LastName = trainee.LastName;
            updatedTrainee.Email = trainee.Email;
            updatedTrainee.TechStack = trainee.TechStack;
            updatedTrainee.Status = trainee.Status;
            updatedTrainee.UpdatedDate = DateTime.UtcNow;

            await _traineeRepository.UpdateAsync(updatedTrainee);
            return MapToResponse(updatedTrainee);
        }

        public async Task<bool> Delete(int Id)
        {
            var trainee = await _traineeRepository.GetByIdAsync(Id);
            if (trainee == null)
                return true;

            await _traineeRepository.DeleteAsync(trainee);
            return true;
        }

        private TraineeResponseDTO MapToResponse(Trainee trainee)
        {
            return new TraineeResponseDTO
            {
                Id = trainee.Id,
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

