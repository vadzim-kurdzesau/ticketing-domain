using IWent.Services.DTO.Events;

namespace IWent.Services;

/// <summary>
/// Provides functionality for API to operate with events.
/// </summary>
public interface IEventsService
{
    /// <summary>
    /// Gets the specified <paramref name="amount"/> of events starting from the specified <paramref name="page"/>.
    /// </summary>
    Task<IEnumerable<Event>> GetEventsAsync(int page, int amount, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all seats of the event manifest's section with the specified <paramref name="sectionId"/>.
    /// </summary>
    Task<IEnumerable<SectionSeat>> GetSectionSeats(int eventId, int sectionId, CancellationToken cancellationToken);
}
