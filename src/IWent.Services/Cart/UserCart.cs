using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IWent.Services.Cart;

/// <summary>
/// Represents a list of user order's seats.
/// </summary>
public class UserCart : IEnumerable<CartItem>
{
    private readonly ConcurrentDictionary<CartItemKey, CartItem> _items = new ConcurrentDictionary<CartItemKey, CartItem>();

    /// <summary>
    /// The unique identifier of the cart.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// The number of items in the cart.
    /// </summary>
    public int ItemsTotal => _items.Count;

    /// <summary>
    /// Tries to add <paramref name="item"/> to the cart.
    /// </summary>
    /// <returns>True, if <paramref name="item"/> was successfully added; false otherwise.</returns>
    public bool TryAddItem(CartItem item)
        => _items.TryAdd(key: new CartItemKey(item.SeatId, item.EventId), item);

    /// <summary>
    /// Tries to remove the item with the specified <paramref name="seatId"/> from the cart.
    /// </summary>
    /// <returns>True, if item was successfully removed; false otherwise.</returns>
    public bool TryRemove(CartItemKey key)
        => _items.TryRemove(key, out var _);

    public IEnumerator<CartItem> GetEnumerator()
        => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}

public record CartItemKey(int SeatId, int EventId);