using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task RemoveAsync(string key);
    }
}