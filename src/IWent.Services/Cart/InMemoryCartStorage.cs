using System.Collections.Concurrent;
using IWent.Services.Exceptions;

namespace IWent.Services.Cart;

public class InMemoryCartStorage : ICartStorage
{
    private readonly ConcurrentDictionary<string, UserCart> _cache = new();

    public UserCart Get(string cartId)
    {
        return _cache.TryGetValue(cartId, out var cart)
            ? cart
            : throw new ResourceDoesNotExistException($"Cart with the ID '{cartId}' does not exist.");
    }

    public UserCart GetOrCreate(string cartId)
    {
        return _cache.GetOrAdd(cartId, new UserCart { Id = cartId });
    }

    public void Remove(string cartId)
    {
        _cache.TryRemove(cartId, out var _);
    }
}
