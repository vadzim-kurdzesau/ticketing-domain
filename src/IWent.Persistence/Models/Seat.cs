namespace IWent.Persistence.Models;

/// <summary>
/// Represents the smallest manifest unit that can be purchased or booked.
/// </summary>
public class Seat
{
    // TODO: add the ID

    /// <summary>
    /// The name of the seat.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The state of the seat.
    /// </summary>
    public SeatState State { get; set; }
}
