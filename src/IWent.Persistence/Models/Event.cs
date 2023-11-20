using System;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents an occurrence in time, such as a sports event.
/// </summary>
public class Event
{
    /// <summary>
    /// The unique identifier of the event.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the event.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The date and time when this event is happening (in UTC).
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The unique identifier of the venue where this event is happening.
    /// </summary>
    public int VenueId { get; set; }

    /// <summary>
    /// The venue where this event is happening.
    /// </summary>
    public Venue Venue { get; set; } = null!;
}
