using System.Threading;
using System.Threading.Tasks;

namespace IWent.Services.Caching;

public class NullCacheService<T> : ICacheService<T>
{
    public Task AddAsync(string key, T value, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync(string key, CancellationToken cancellationToken)
    {
        return Task.FromResult(default(T?));
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
