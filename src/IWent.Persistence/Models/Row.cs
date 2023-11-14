using System.Collections.Generic;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents a row of seats within a section.
/// </summary>
public class Row
{
    // TODO: add the ID

    /// <summary>
    /// The name of the row.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The seats within the row.
    /// </summary>
    public IEnumerable<Seat> Seats { get; set; } = null!;
}
