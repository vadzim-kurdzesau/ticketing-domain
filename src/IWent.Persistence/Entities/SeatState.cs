namespace IWent.Persistence.Entities;

/// <summary>
/// A state of a seat.
/// </summary>
public enum SeatStatus
{
    /// <summary>
    /// Seat is unavailable for booking.
    /// </summary>
    Unvailable,

    /// <summary>
    /// Seat is unavailable for booking.
    /// </summary>
    Available,

    /// <summary>
    /// Seat has been booked an currently is unavailable.
    /// </summary>
    Booked,

    /// <summary>
    /// Seat have been purchased and is unavailable.
    /// </summary>
    Sold
}

/// <summary>
/// Represents the possible state of a seat.
/// </summary>
public class SeatState
{
    /// <summary>
    /// The unique identifier of the seat state.
    /// </summary>
    public SeatStatus Id { get; set; }

    /// <summary>
    /// The name of the state.
    /// </summary>
    public string Name { get; set; } = null!;
}
