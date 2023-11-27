using System;

namespace IWent.Services.Orders;

/// <summary>
/// 
/// </summary>
public readonly struct CartItem
{
    public CartItem(int eventId, int seatId, int priceId, DateTime addedAt)
    {
        EventId = eventId;
        SeatId = seatId;
        PriceId = priceId;
        AddedAt = addedAt;
    }

    /// <summary>
    /// The identifier of the event this seat is booked for.
    /// </summary>
    public int EventId { get; }

    /// <summary>
    /// The identifier of the booked seat.
    /// </summary>
    public int SeatId { get; }

    /// <summary>
    /// The identifier of the booked seat's price.
    /// </summary>
    public int PriceId { get; }

    /// <summary>
    /// The date and time when this seat was added to the cart.
    /// </summary>
    public DateTime AddedAt { get; }
}
