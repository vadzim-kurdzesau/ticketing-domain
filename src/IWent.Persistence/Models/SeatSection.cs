using System.Collections.Generic;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents a physical group of seats of a same type.
/// </summary>
public class SeatSection
{
    // TODO: add the ID

    /// <summary>
    /// The name of this section.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of seats in this section.
    /// </summary>
    public SeatType SeatType { get; set; }

    /// <summary>
    /// The list this section's seats in case its <see cref="SeatType"/> is a General Admission.
    /// </summary>
    public IEnumerable<Seat>? Seats { get; set; }

    /// <summary>
    /// The list of rows of seats in this section in case its <see cref="SeatType"/> is a Designated.
    /// </summary>
    public IEnumerable<Row>? Rows { get; set; }
}
