using System;
using System.Collections.Generic;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents a physical group of seats of a same type.
/// </summary>
public class Section
{
    /// <summary>
    /// The identifier of the venue containing this section.
    /// </summary>
    public int VenueId { get; set; }

    /// <summary>
    /// The unique name of this section.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of seats in this section.
    /// </summary>
    public SeatType SeatType { get; set; }

    /// <summary>
    /// The list this section's rows.
    /// </summary>
    public IEnumerable<Row> Rows { get; set; } = Array.Empty<Row>();
}
