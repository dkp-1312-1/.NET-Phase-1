using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
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
            string cacheKey = StringConstants.trainee(StringConstants.all);
            var trainees = await _cacheService.GetAsync<List<Trainee>>(cacheKey);
            var totalRecords=trainees?.Count()??0;
            if (trainees == null)
            {
                (trainees, totalRecords) = await _traineeRepository.GetTraineesAsync(trainee);
            }
            var result = new PagedResponseDTO<TraineeResponseDTO>
            {
                PageNumber = trainee.PageNumber,
                PageSize = trainee.PageSize,
                TotalRecords = totalRecords,
                Data = trainees.Select(MapToResponse)
            };
            await _cacheService.SetAsync(cacheKey, trainees, TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));

            return result;
        }

        public async Task<TraineeResponseDTO> GetById(int Id)
        {
            string cacheKey = StringConstants.trainee(Id);
            var cachedTrainee = await _cacheService.GetAsync<TraineeResponseDTO>(cacheKey);
            if (cachedTrainee != null)
            {
                return cachedTrainee;
            }
            var trainee = await _traineeRepository.GetByIdAsync(Id);
            await _cacheService.SetAsync(cacheKey, MapToResponse(trainee), TimeSpan.FromMinutes(IntConstants.CacheTimeLimit));
            return trainee != null ? MapToResponse(trainee) : null;
        }

        public async Task<TraineeResponseDTO> Create(CreateTraineeRequestDTO trainee)
        {
            string cacheKey = StringConstants.trainee(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
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
            string cacheKey = StringConstants.trainee(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
            string cacheKeyId = StringConstants.trainee(Id);
            await _cacheService.RemoveAsync(cacheKeyId);
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
            string cacheKey = StringConstants.trainee(StringConstants.all);
            await _cacheService.RemoveAsync(cacheKey);
            string cacheKeyId = StringConstants.trainee(Id);
            await _cacheService.RemoveAsync(cacheKeyId);
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

