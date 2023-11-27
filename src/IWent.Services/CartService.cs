﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Persistence.Entities;
using IWent.Services.DTO;
using IWent.Services.DTO.Orders;
using IWent.Services.Exceptions;
using IWent.Services.Orders;

namespace IWent.Services;

public class CartService : IOrdersService
{
    private readonly ICartStorage _cartStorage;
    private readonly EventContext _eventContext;

    public CartService(ICartStorage cartStorage, EventContext eventContext)
    {
        _cartStorage = cartStorage;
        _eventContext = eventContext;
    }

    public IEnumerable<DTO.Orders.OrderItem> GetItemsInCart(string cartId)
    {
        var cart = _cartStorage.Get(cartId);
        return cart.Select(i => ToDTO(in i));
    }

    public CartState AddToCart(string cartId, DTO.Orders.OrderItem orderItem)
    {
        var cart = _cartStorage.GetOrCreate(cartId);
        if (!cart.TryAddItem(new CartItem(orderItem.EventId, orderItem.SeatId, orderItem.PriceId, addedAt: DateTime.UtcNow)))
        {
            throw new CartAlreadyExistsException($"Cart with the '{cartId}' id already exists.");
        }

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

        var seatIds = cartSeats.Select(s => s.SeatId);

        var seats = _eventContext.Seats
            .Where(s => seatIds.Contains(s.Id));

        foreach (var seat in seats)
        {
            seat.State = SeatState.Booked;
        }

        await _eventContext.SaveChangesAsync(cancellationToken);

        _cartStorage.Remove(cartId);

        throw new NotImplementedException();
    }

    private static DTO.Orders.OrderItem ToDTO(in CartItem item)
        => new()
        {
            EventId = item.EventId,
            SeatId = item.SeatId,
            PriceId = item.PriceId,
        };
}
