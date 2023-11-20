using System;
using System.Collections.Generic;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents a physical row of seats within a section.
/// </summary>
public class Row
{
    /// <summary>
    /// The unique identifier of the row.
    /// </summary>
    public int RowId { get; set; }

    /// <summary>
    /// The identifier of the section containing this row.
    /// </summary>
    public int SectionId { get; set; }

    /// <summary>
    /// The list this row's seats.
    /// </summary>
    public IEnumerable<Seat> Rows { get; set; } = Array.Empty<Seat>();
}
