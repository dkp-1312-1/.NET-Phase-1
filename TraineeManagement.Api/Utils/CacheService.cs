using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TraineeManagement.Api.Resources;
namespace TraineeManagement.Api.Utils
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                _logger.LogInformation("Cache GetAsync");
                string? cachedData = await _cache.GetStringAsync(key);
                if (cachedData == null)
                    return default;
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis Cache failure on GET for key: {Key}", key);
                return default;
            }
        }
        public async Task SetAsync<T>(string key, T value)
        {
            try
            {
                DistributedCacheEntryOptions options =new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow=TimeSpan.FromMinutes(Config.RedisCacheTimeLimit)
                };
                string serializedData =JsonSerializer.Serialize(value);
                _logger.LogInformation("Cache SetAsync");
                await _cache.SetStringAsync(key,serializedData,options);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Redis Cache failure on SET for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                _logger.LogInformation("Cache RemoveAsync");
                await _cache.RemoveAsync(key);
            }
            catch(Exception ex)
            {
                  _logger.LogError(ex, "Redis Cache failure on REMOVE for key: {Key}", key);
            }
        }
    }
}