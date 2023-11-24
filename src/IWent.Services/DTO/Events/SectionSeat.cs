namespace IWent.Services.DTO.Events;

/// <summary>
/// Represents the smallest manifest unit that can be purchased or booked.
/// </summary>
public class SectionSeat
{
    /// <summary>
    /// The identifier of the section containing this row.
    /// </summary>
    public int SectionId { get; set; }

    /// <summary>
    /// The identifier of the row containing this seat.
    /// </summary>
    public int RowId { get; set; }

    /// <summary>
    /// The unique identifier of the seat.
    /// </summary>
    public int SeatId { get; set; }

    /// <summary>
    /// The unique number of this seat in this section.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// The state of the seat.
    /// </summary>
    public SeatState State { get; set; }

    /// <summary>
    /// The available price options for this seat.
    /// </summary>
    public IEnumerable<PriceOption>? PriceOptions { get; set; }
}
