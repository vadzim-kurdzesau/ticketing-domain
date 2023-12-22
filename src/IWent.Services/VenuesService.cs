using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Services.DTO.Common;
using IWent.Services.DTO.Venues;
using Microsoft.EntityFrameworkCore;

namespace IWent.Services;

public class VenuesService : IVenuesService
{
    private readonly EventContext _eventContext;

    public VenuesService(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public async Task<IEnumerable<Venue>> GetVenuesAsync(int page, int amount, CancellationToken cancellationToken)
    {
        var venues = await _eventContext.Venues.OrderBy(v => v.Name)
            .Skip((page - 1) * amount)
            .Take(amount)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return venues.Select(ToDTO);
    }

    public async Task<IEnumerable<VenueSection>> GetSectionsAsync(int venueId, CancellationToken cancellationToken)
    {
        var sections = await _eventContext.Sections
            .Where(s => s.VenueId == venueId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return sections.Select(ToDTO);
    }

    private static Venue ToDTO(Persistence.Entities.Venue venue)
        => new Venue
        {
            Id = venue.Id,
            Name = venue.Name,
            Address = new Address
            {
                Country = venue.Country,
                Region = venue.Region,
                City = venue.City,
                Street = venue.Street,
            }
        };

    private static VenueSection ToDTO(Persistence.Entities.Section section)
        => new VenueSection
        {
            Id = section.Id,
            Name = section.Name,
            SeatType = (SeatType)section.SeatType
        };
}
