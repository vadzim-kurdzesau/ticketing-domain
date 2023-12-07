namespace IWent.Persistence.Entities;

/// <summary>
/// Represent an item of the order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// The unique identifier of the payment this item is related to.
    /// </summary>
    public string PaymentId { get; set; } = null!;

    /// <summary>
    /// The unique identifier of the seat added to this order.
    /// </summary>
    public int SeatId { get; set; }

    /// <summary>
    /// The unique identifier of the event.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// The unique identifier of the chosen for this seat price option.
    /// </summary>
    public int PriceId { get; set; }

    /// <summary>
    /// The payment this item is related to.
    /// </summary>
    public Payment Payment { get; set; } = null!;

    /// <summary>
    /// The seat added to this order.
    /// </summary>
    public EventSeat Seat { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public Event Event { get; set; }

    /// <summary>
    /// The chosen for this seat price option.
    /// </summary>
    public Price Price { get; set; } = null!;
}
