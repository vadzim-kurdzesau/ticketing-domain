using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.BookingTimer.Messages;
using IWent.Persistence;
using IWent.Persistence.Entities;
using IWent.Services.Caching;
using IWent.Services.DTO;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using IWent.Services.Exceptions;
using IWent.Services.Extensions;
using IWent.Services.Notifications;
using IWent.Services.Notifications.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IWent.Services.Cart;

public class CartService : ICartService
{
    private readonly ICartStorage _cartStorage;
    private readonly EventContext _eventContext;
    private readonly ICacheService<Event> _cache;
    private readonly INotificationClient _notificationClient;
    private readonly IBusConnectionConfiguration _busConfiguration;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ICartStorage cartStorage,
        EventContext eventContext,
        ICacheService<Event> cache,
        INotificationClient notificationClient,
        IBusConnectionConfiguration busConfiguration,
        ILogger<CartService> logger)
    {
        _cartStorage = cartStorage;
        _eventContext = eventContext;
        _cache = cache;
        _notificationClient = notificationClient;
        _busConfiguration = busConfiguration;
        _logger = logger;
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
            throw new SeatAlreadyBookedException($"Seat  with the id '{orderItem.SeatId}' for the event with the id '{orderItem.EventId}' already has been booked.");
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
        if (!cart.TryRemove(new CartItemKey(seatId, eventId)))
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

        var seatsByEvents = cartSeats.GroupBy(s =>  s.EventId);

        Payment order;
        using (var transaction = _eventContext.Database.BeginTransaction())
        {
            var orderItems = new List<Persistence.Entities.OrderItem>(cartSeats.Length);
            foreach (var seatGroup in seatsByEvents)
            {
                var bookedSeatIds = seatGroup.ToDictionary(i => i.SeatId);

                var bookedEventSeats = await _eventContext.EventSeats
                    .Where(s => s.EventId == seatGroup.Key && bookedSeatIds.Keys.Contains(s.SeatId))
                    .ToListAsync(cancellationToken);

                foreach (var bookedSeat in bookedEventSeats)
                {
                    if (bookedSeat.StateId != SeatStatus.Available)
                    {
                        throw new InvalidOperationException($"Cannot book a seat with the id '{bookedSeat.SeatId}' because it's has already been booked.");
                    }

                    bookedSeat.StateId = SeatStatus.Booked;
                }

                orderItems.AddRange(bookedEventSeats.Select(s => new Persistence.Entities.OrderItem
                {
                    SeatId = s.SeatId,
                    EventId = s.EventId,
                    PriceId = bookedSeatIds[s.SeatId].PriceId,
                }));
            }

            order = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Status = Persistence.Entities.PaymentStatus.Pending,
                OrderItems = orderItems,
            };

            _eventContext.Payments.Add(order);
            await _eventContext.SaveChangesAsync(cancellationToken);

            transaction.Commit();
        }

        try
        {
            _cartStorage.Remove(cartId);
            var eventsToInvalidateIds = cartSeats.Select(s => s.EventId).Distinct().ToArray();
            foreach (var task in eventsToInvalidateIds.Select(i => _cache.RemoveAsync(cartId, cancellationToken)))
            {
                await task;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown during the '{CartId}' cache cleaning.", cartId);
        }

        var startTimerMessage = new BookingTimerMessage
        {
            BookingNumber = order.Id,
            Action = TimerAction.Start
        };

        await _notificationClient.SendMessageAsync(startTimerMessage, _busConfiguration.BookingTimersQueueName, cancellationToken);

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
