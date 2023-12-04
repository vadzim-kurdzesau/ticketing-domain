using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IWent.Services.Caching;

public class CacheService<T> : ICacheService<T>
{
    private readonly IDistributedCache _cache;
    private readonly ICacheConfiguration _cacheConfiguration;

    public CacheService(IDistributedCache cache, ICacheConfiguration cacheConfiguration)
    {
        _cache = cache;
        _cacheConfiguration = cacheConfiguration;
    }

    public Task AddAsync(string key, T value, CancellationToken cancellationToken)
    {
        var serializedEntry = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });

        return _cache.SetStringAsync(key, serializedEntry, new DistributedCacheEntryOptions
        {
            SlidingExpiration = _cacheConfiguration.SlidingExpiration
        }, cancellationToken);
    }

    public async Task<T?> GetAsync(string key, CancellationToken cancellationToken)
    {
        var serializedEntry = await _cache.GetStringAsync(key, cancellationToken);
        if (serializedEntry == null)
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(serializedEntry);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }
}
