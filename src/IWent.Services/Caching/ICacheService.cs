using System.Threading;
using System.Threading.Tasks;

namespace IWent.Services.Caching;

public interface ICacheService<T>
{
    Task AddAsync(string key, T value, CancellationToken cancellationToken);

    Task<T?> GetAsync(string key, CancellationToken cancellationToken);

    Task RemoveAsync(string key, CancellationToken cancellationToken);
}