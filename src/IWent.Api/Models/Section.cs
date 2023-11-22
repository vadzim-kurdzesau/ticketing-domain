namespace IWent.Api.Models;

public class Section
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
    /// The identifier of the venue containing this section.
    /// </summary>
    public int VenueId { get; set; }

    /// <summary>
    /// The type of seats in this section.
    /// </summary>
    public SeatType SeatType { get; set; }
}
