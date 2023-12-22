namespace IWent.Services.DTO.Orders;

/// <summary>
/// Represents the item of an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// The identifier of the event this seat is booked for.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// The identifier of the booked seat.
    /// </summary>
    public int SeatId { get; set; }

    /// <summary>
    /// The identifier of the booked seat's price.
    /// </summary>
    public int PriceId { get; set; }
}
