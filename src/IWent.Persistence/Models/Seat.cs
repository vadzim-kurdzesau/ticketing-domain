using System.ComponentModel.DataAnnotations.Schema;

namespace IWent.Persistence.Models;

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
    [Column("row_id")]
    public int RowId { get; set; }

    /// <summary>
    /// The state of the seat.
    /// </summary>
    public SeatState State { get; set; }

    /// <summary>
    /// The identifier of the price offer for which this seat is being sold.
    /// </summary>
    public int PriceId { get; set; }

    /// <summary>
    /// The price offer for which this seat is being sold.
    /// </summary>
    public Price Price { get; set; } = null!;
}
