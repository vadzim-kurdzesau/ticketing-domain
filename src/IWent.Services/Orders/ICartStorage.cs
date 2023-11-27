using System.Diagnostics.CodeAnalysis;

namespace IWent.Services.Orders;

/// <summary>
/// 
/// </summary>
public interface ICartStorage
{
    /// <summary>
    /// Gets stored <see cref="UserCart"/> with the specified <paramref name="cartId"/>.
    /// </summary>
    UserCart Get(string cartId);

    /// <summary>
    /// Gets existing or creates a new <see cref="UserCart"/> with the specified <paramref name="cartId"/>.
    /// </summary>
    UserCart GetOrCreate(string cartId);

    /// <summary>
    /// Removes cart with the specified <paramref name="cartId"/>.
    /// </summary>
    void Remove(string cartId);
}
