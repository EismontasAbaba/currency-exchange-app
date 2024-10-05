using Exchange.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Exchange.Service.Services;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> funcToGetData, int durationInMinutes = 60)
    {
        var cachedData = memoryCache.Get<T>(key);

        if (cachedData != null)
        {
            return cachedData;
        }

        var dataToCache = await funcToGetData();

        if (dataToCache == null)
        {
            return default;
        }

        memoryCache.Set(key, dataToCache, DateTimeOffset.UtcNow.AddMinutes(durationInMinutes));

        return dataToCache;
    }
}