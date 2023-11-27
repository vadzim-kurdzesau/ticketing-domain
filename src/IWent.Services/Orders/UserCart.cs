using System.Collections;
using System.Collections.Generic;

namespace IWent.Services.Orders;

public class UserCart : IEnumerable<CartItem>
{
    private readonly IDictionary<int, CartItem> _items = new Dictionary<int, CartItem>();

    /// <summary>
    /// 
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public int ItemsTotal => _items.Count;

    /// <summary>
    /// Tries to add <paramref name="item"/> to the cart.
    /// </summary>
    /// <returns>True, if <paramref name="item"/> was successfully added; false otherwise.</returns>
    public bool TryAddItem(CartItem item)
        => _items.TryAdd(item.SeatId, item);

    /// <summary>
    /// Tries to remove the item with the specified <paramref name="seatId"/> from the cart.
    /// </summary>
    /// <returns>True, if item was successfully removed; false otherwise.</returns>
    public bool TryRemove(int seatId)
        => _items.Remove(seatId);

    public IEnumerator<CartItem> GetEnumerator()
        => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
