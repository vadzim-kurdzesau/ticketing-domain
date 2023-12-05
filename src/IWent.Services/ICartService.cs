using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;

namespace IWent.Services.DTO;

/// <summary>
/// Provides functionality for API to operate with carts.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Gets all items in the cart with the specified <paramref name="cartId"/>.
    /// </summary>
    IEnumerable<OrderItem> GetItemsInCart(string cartId);

    /// <summary>
    /// Adds <paramref name="orderItem"/> to the cart with the specified <paramref name="cartId"/>.
    /// </summary>
    Task<CartState> AddToCartAsync(string cartId, OrderItem orderItem, CancellationToken cancellationToken);

    /// <summary>
    /// Removes seat with the specified <paramref name="seatId"/> from the cart with the <paramref name="cartId"/>.
    /// </summary>
    void RemoveFromCart(string cartId, int eventId, int seatId);

    /// <summary>
    /// Books all seats in the cart with the <paramref name="cartId"/>.
    /// </summary>
    Task<PaymentInfo> BookSeatsAsync(string cartId, CancellationToken cancellationToken);
}
