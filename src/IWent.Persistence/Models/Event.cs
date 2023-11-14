using System;

namespace IWent.Persistence.Models;

/// <summary>
/// Represents an occurrence in time, such as a sports event.
/// </summary>
public class Event
{
    /// <summary>
    /// The name of the event.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The date and time when this event is happening.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The venue where this event is happening.
    /// </summary>
    public Venue Venue { get; set; } = null!;

    /// <summary>
    /// The seating arrangement associated with this event.
    /// </summary>
    public Manifest Manifest { get; set; } = null!;
}
