using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IWent.Persistence.Entities;

/// <summary>
/// Represents a seat's status at the specified event.
/// </summary>
public class EventSeat
{
    /// <summary>
    /// The unique identifier of the seat.
    /// </summary>
    public int SeatId { get; set; }

    /// <summary>
    /// The unique identifier of the event this seat is related to.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// The unique identifier of the state of the seat.
    /// </summary>
    public SeatStatus StateId { get; set; }

    /// <summary>
    /// The state of the seat.
    /// </summary>
    public SeatState State { get; set; } = null!;

    /// <summary>
    /// The seat.
    /// </summary>
    public Seat Seat { get; set; } = null!;

    /// <summary>
    /// The event this seat is related to.
    /// </summary>
    public Event Event { get; set; } = null!;

    /// <summary>
    /// The concurrency token used to .
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// The price offers for which this seat is being sold.
    /// </summary>
    public IEnumerable<Price> PriceOptions { get; set; } = null!;
}
