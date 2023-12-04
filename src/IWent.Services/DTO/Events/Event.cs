using System;
using IWent.Services.DTO.Common;

namespace IWent.Services.DTO.Events;

/// <summary>
/// Represents an occurrence in time, such as a sports event and where it happens.
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
    /// The unique identifier of the venue this event is happening.
    /// </summary>
    public int VenueId { get; set; }

    /// <summary>
    /// The location where this event is happening.
    /// </summary>
    public Address Address { get; set; } = null!;
}
