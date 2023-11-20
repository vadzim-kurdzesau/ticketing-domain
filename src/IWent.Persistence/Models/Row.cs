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
    public int Id { get; set; }

    /// <summary>
    /// The name of the row.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The identifier of the section containing this row.
    /// </summary>
    public int SectionId { get; set; }

    /// <summary>
    /// The section containing this row.
    /// </summary>
    public Section Section { get; set; } = null!;

    /// <summary>
    /// The list this row's seats.
    /// </summary>
    public IEnumerable<Seat> Seats { get; set; } = Array.Empty<Seat>();
}
