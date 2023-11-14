using System.Collections.Generic;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents the seating arrangement applicable to a particular venue or manifest.
/// </summary>
public class Manifest
{
    /// <summary>
    /// The sections within the manifest.
    /// </summary>
    public IEnumerable<SeatSection> Sections { get; set; } = null!;
}
