using IWent.Services.DTO.Common;

namespace IWent.Services.DTO.Venues;

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
    /// The location of this venue.
    /// </summary>
    public Address Address { get; set; } = null!;
}
