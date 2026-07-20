using TraineeManagement.Api.Utils;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;
using TraineeManagement.Api.Resources;
using System.Text.Json;

namespace TraineeManagement.Api.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ITraineeRepository _traineeRepository;
        private readonly ICacheService _cacheService;

        public TraineeService(ITraineeRepository traineeRepository, ICacheService cacheService)
        {
            _traineeRepository = traineeRepository;
            _cacheService = cacheService;
        }

        public async Task<PagedResponseDTO<TraineeResponseDTO>> GetAll(SearchDTO<TraineeStatusType> trainee)
        {
            (List<Trainee>? trainees, int totalRecords) = await _traineeRepository.GetTraineesAsync(trainee);
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
            string cacheKey = StringConstants.trainee(Id);
            TraineeResponseDTO cachedTrainee = await _cacheService.GetAsync<TraineeResponseDTO>(cacheKey);
            if (cachedTrainee != null)
            {
                return cachedTrainee;
            }
            Trainee? trainee = await _traineeRepository.GetByIdAsync(Id);
            if(trainee != null)
                await _cacheService.SetAsync(cacheKey, MapToResponse(trainee));
            return trainee != null ? MapToResponse(trainee) : null!;
        }

        public async Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO trainee)
        {
            Trainee newTrainee = new Trainee(trainee);

            await _traineeRepository.AddAsync(newTrainee);
            return MapToResponse(newTrainee);
        }

        public async Task<TraineeResponseDTO> Update(int Id, UpdateTraineeRequestDTO trainee)
        {
            string cacheKeyId = StringConstants.trainee(Id);
            await _cacheService.RemoveAsync(cacheKeyId);
            Trainee updatedTrainee = await _traineeRepository.GetByIdAsync(Id);
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
            string cacheKeyId = StringConstants.trainee(Id);
            await _cacheService.RemoveAsync(cacheKeyId);
            Trainee trainee = await _traineeRepository.GetByIdAsync(Id);
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


