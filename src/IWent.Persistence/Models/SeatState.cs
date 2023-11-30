namespace IWent.Persistence.Models;

/// <summary>
/// A state of a seat.
/// </summary>
public enum SeatState
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
