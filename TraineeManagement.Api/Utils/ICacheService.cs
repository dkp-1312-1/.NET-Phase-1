using TraineeManagement.Data.Models;
using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Api.Utils
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task RemoveAsync(string key);
    }
}
