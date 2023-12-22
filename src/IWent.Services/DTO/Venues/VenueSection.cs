namespace IWent.Services.DTO.Venues;

/// <summary>
/// Represents a physical group of seats of a same type.
/// </summary>
public class VenueSection
{
    /// <summary>
    /// The unique identifier of the section.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of this section.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of seats in this section.
    /// </summary>
    public SeatType SeatType { get; set; }
}
