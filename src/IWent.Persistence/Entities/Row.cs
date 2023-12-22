using System.Collections.Generic;

namespace IWent.Persistence.Entities;

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
    /// The number of the row.
    /// </summary>
    public int Number { get; set; }

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
    public IEnumerable<Seat> Seats { get; set; } = null!;
}
