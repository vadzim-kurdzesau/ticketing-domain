using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Services.DTO.Common;
using IWent.Services.DTO.Events;
using Microsoft.EntityFrameworkCore;

namespace IWent.Services;

public class EventsService : IEventsService
{
    private readonly EventContext _eventContext;

    public EventsService(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(int page, int amount, CancellationToken cancellationToken)
    {
        var events = await _eventContext.Events.OrderByDescending(e => e.Date)
            .Skip((page - 1) * amount)
            .Take(amount)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return events.Select(ToDTO);
    }
    
    public async Task<IEnumerable<SectionSeat>> GetSectionSeats(int eventId, int sectionId, CancellationToken cancellationToken)
    {
        var seats = await _eventContext.Seats
            .Include(s => s.Row)
            .Include(s => s.PriceOptions)
            .Where(s => s.Row.SectionId == sectionId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return seats.Select(ToDTO);
    }

    private static Event ToDTO(Persistence.Entities.Event @event)
        => new Event
        {
            Id = @event.Id,
            Name = @event.Name,
            Date = @event.Date,
            Address = new Address
            {
                Country = @event.Venue.Country,
                Region = @event.Venue.Region,
                City = @event.Venue.City,
                Street = @event.Venue.Street,
            }
        };

    private static SectionSeat ToDTO(Persistence.Entities.Seat seat)
        => new SectionSeat
        {
            SectionId = seat.Row.SectionId,
            RowId = seat.RowId,
            SeatId = seat.Id,
            Number = seat.Number,
            State = (SeatState)seat.State,
            PriceOptions = seat.PriceOptions.Select(o => new PriceOption
            {
                Id = o.Id,
                Name = o.Name,
            }).ToArray(),
        };
}
