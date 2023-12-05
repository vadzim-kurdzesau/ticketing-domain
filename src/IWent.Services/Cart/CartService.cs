using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Persistence.Entities;
using IWent.Services.Caching;
using IWent.Services.DTO;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using IWent.Services.Exceptions;
using IWent.Services.Extensions;

namespace IWent.Services.Cart;

public class CartService : ICartService
{
    private readonly ICartStorage _cartStorage;
    private readonly EventContext _eventContext;
    private readonly ICacheService<Event> _cache;

    public CartService(ICartStorage cartStorage, EventContext eventContext, ICacheService<Event> cache)
    {
        _cartStorage = cartStorage;
        _eventContext = eventContext;
        _cache = cache;
    }

    public IEnumerable<DTO.Orders.OrderItem> GetItemsInCart(string cartId)
    {
        var cart = _cartStorage.Get(cartId);
        return cart.Select(i => ToDTO(in i));
    }

    public async Task<CartState> AddToCartAsync(string cartId, DTO.Orders.OrderItem orderItem, CancellationToken cancellationToken)
    {
        var seat = await _eventContext.EventSeats.FindAsync(new object[] { orderItem.SeatId, orderItem.EventId }, cancellationToken);
        if (seat == null)
        {
            throw new ResourceDoesNotExistException($"Seat with the id '{orderItem.SeatId}' for the event with the id '{orderItem.EventId}' does not exist.");
        }

        if (seat.StateId != SeatStatus.Available)
        {
            throw new ApiException($"Seat  with the id '{orderItem.SeatId}' for the event with the id '{orderItem.EventId}' already has been booked.");
        }

        var cart = _cartStorage.GetOrCreate(cartId);
        cart.TryAddItem(new CartItem(orderItem.EventId, orderItem.SeatId, orderItem.PriceId, addedAt: DateTime.UtcNow));

        return new CartState
        {
            ItemsTotal = cart.ItemsTotal,
        };
    }

    public void RemoveFromCart(string cartId, int eventId, int seatId)
    {
        var cart = _cartStorage.Get(cartId);
        if (!cart.TryRemove(seatId))
        {
            throw new ResourceDoesNotExistException($"Item with the specified '{eventId}' event and '{seatId}' seat ids doesn't exist.");
        }
    }

    public async Task<PaymentInfo> BookSeatsAsync(string cartId, CancellationToken cancellationToken)
    {
        var cart = _cartStorage.Get(cartId);
        var cartSeats = cart.ToArray();
        if (cartSeats.Length == 0)
        {
            throw new CartIsEmptyException($"Cart '{cartId}' is empty.");
        }

        var seatIds = cartSeats.ToDictionary(s => s.SeatId);

        Payment order;
        using (var transaction = _eventContext.Database.BeginTransaction())
        {
            var seats = _eventContext.EventSeats
                .Where(s => seatIds.Keys.Contains(s.SeatId));

            foreach (var seat in seats)
            {
                if (seat.StateId != SeatStatus.Available)
                {
                    throw new InvalidOperationException($"Cannot book a seat with the id '{seat.SeatId}' because it's has already been booked.");
                }

                seat.StateId = SeatStatus.Booked;
            }

            order = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Status = Persistence.Entities.PaymentStatus.Pending,
                OrderItems = seats.Select(s => new Persistence.Entities.OrderItem
                {
                    SeatId = s.SeatId,
                    EventId = s.EventId,
                    PriceId = seatIds[s.SeatId].PriceId,
                }).ToList(),
            };

            _eventContext.Payments.Add(order);
            await _eventContext.SaveChangesAsync(cancellationToken);

            transaction.Commit();
        }

        _cartStorage.Remove(cartId);
        var eventsToInvalidateIds = cartSeats.Select(s => s.EventId).Distinct().ToArray();
        await Task.WhenAll(eventsToInvalidateIds.Select(i => _cache.RemoveAsync(cartId, cancellationToken)));

        return new PaymentInfo
        {
            PaymentId = order.Id,
            Status = order.Status.ToDTO(),
        };
    }

    private static DTO.Orders.OrderItem ToDTO(in CartItem item)
        => new()
        {
            EventId = item.EventId,
            SeatId = item.SeatId,
            PriceId = item.PriceId,
        };
}
