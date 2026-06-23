using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null);
        Task RemoveAsync(string key);
    }
}