using IWent.Persistence;
using IWent.Services.DTO.Venues;
using Microsoft.EntityFrameworkCore;

namespace IWent.Services;

public class VenueService : IVenueService
{
    private readonly EventContext _eventContext;

    public VenueService(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public async Task<IEnumerable<Venue>> GetVenuesAsync(int page, int size, CancellationToken cancellationToken)
    {
        var venues = await _eventContext.Venues.OrderBy(v => v.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return venues.Select(v => new Venue
        {
            Id = v.Id,
            Name = v.Name,
            Country = v.Country,
            Region = v.Region,
            City = v.City,
            Street = v.Street,
        });
    }

    public async Task<IEnumerable<VenueSection>> GetSectionsAsync(int venueId, CancellationToken cancellationToken)
    {
        var sections = await _eventContext.Sections
            .Where(s => s.VenueId == venueId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return sections.Select(s => new VenueSection
        {
            Id = s.Id,
            Name = s.Name,
            SeatType = (SeatType)s.SeatType
        });
    }
}
