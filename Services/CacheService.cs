using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TraineeManagement.Api.Resources;
namespace TraineeManagement.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = _logger;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                Console.WriteLine("Cache GetAsync");
                var cachedData = await _cache.GetStringAsync(key);
                if (cachedData == null)
                    return default;
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis Cache failure on GET for key: {Key}", key);
                return default;
            }
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null)
        {
            try
            {
                var options=new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow=absoluteExpireTime??TimeSpan.FromMinutes(IntConstants.CacheTimeLimit)
                };
                var serializedData=JsonSerializer.Serialize(value);
                Console.WriteLine("Cache SetAsync");
                await _cache.SetStringAsync(key,serializedData,options);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Redis Cache failure on SET for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                Console.WriteLine("Cache RemoveAsync");
                await _cache.RemoveAsync(key);
            }
            catch(Exception ex)
            {
                  _logger.LogWarning(ex, "Redis Cache failure on REMOVE for key: {Key}", key);
            }
        }
    }
}