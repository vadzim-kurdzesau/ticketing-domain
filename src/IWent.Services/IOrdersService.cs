using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO.Orders;
using IWent.Services.Orders;

namespace IWent.Services.DTO;

public interface IOrdersService
{
    IEnumerable<OrderItem> GetItemsInCart(string cartId);

    CartState AddToCart(string cartId, OrderItem orderItem);

    void RemoveFromCart(string cartId, int eventId, int seatId);

    Task<PaymentInfo> BookSeatsAsync(string cartId, CancellationToken cancellationToken);
}
