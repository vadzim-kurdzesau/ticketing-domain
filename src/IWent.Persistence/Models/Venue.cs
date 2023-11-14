namespace IWent.Persistence.Models;

/// <summary>
/// Represents a physical place where an event is happening.
/// </summary>
public class Venue
{
    /// <summary>
    /// The name of the venue.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The county name where this venue is located.
    /// </summary>
    public string Country { get; set; } = null!;

    /// <summary>
    /// The region where this venue is located.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// The city where this venue is located.
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// The street on which this venue is located.
    /// </summary>
    public string Street { get; set; } = null!;

    /// <summary>
    /// The seating arrangement of this particular venue (complete seat map).
    /// </summary>
    public Manifest Manifest { get; set; } = null!;
}
