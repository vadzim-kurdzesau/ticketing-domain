namespace IWent.Api.Models;

/// <summary>
/// Represents a physical place where an event is happening.
/// </summary>
public class Venue
{
    /// <summary>
    /// The unique identifier of the venue.
    /// </summary>
    public int Id { get; set; }

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
}
