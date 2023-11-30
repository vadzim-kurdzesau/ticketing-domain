using System.Collections.Generic;

namespace IWent.Persistence.Entities;

/// <summary>
/// Represents the smallest manifest unit that can be purchased or booked.
/// </summary>
public class Seat
{
    /// <summary>
    /// The unique identifier of the seat.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique number of this seat in this section.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// The identifier of the row containing this seat.
    /// </summary>
    public int RowId { get; set; }

    /// <summary>
    /// The row containing this seat.
    /// </summary>
    public Row Row { get; set; } = null!;

    /// <summary>
    /// The events where this seats is used.
    /// </summary>
    public IEnumerable<EventSeat> EventSeats { get; set; } = null!;
}
