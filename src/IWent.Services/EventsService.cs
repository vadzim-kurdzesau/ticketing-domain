using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Services.Caching;
using IWent.Services.DTO.Common;
using IWent.Services.DTO.Events;
using IWent.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IWent.Services;

public class EventsService : IEventsService
{
    private readonly EventContext _eventContext;
    private readonly ICacheService<Persistence.Entities.Event> _cache;
    private readonly ILogger<EventsService> _logger;

    public EventsService(EventContext eventContext, ICacheService<Persistence.Entities.Event> cache, ILogger<EventsService> logger)
    {
        _eventContext = eventContext;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(int page, int amount, CancellationToken cancellationToken)
    {
        var events = await GetRequestEventDataQuery()
            .OrderBy(e => e.Date)
            .Skip((page - 1) * amount)
            .Take(amount)
            .ToListAsync(cancellationToken);

        try
        {
            var cachingTasks = events.Select(e => _cache.AddAsync(e.Id.ToString(), e, cancellationToken));
            await Task.WhenAll(cachingTasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown while caching the events.");
        }

        return events.Select(ToDTO);
    }
    
    public async Task<IEnumerable<SectionSeat>> GetSectionSeats(int eventId, int sectionId, CancellationToken cancellationToken)
    {
        var cachedEvent = await _cache.GetAsync(eventId.ToString(), cancellationToken);
        if (cachedEvent != null)
        {
            return ToSectionSeatsDTO(cachedEvent, sectionId);
        }

        var @event = await GetRequestEventDataQuery()
            .Where(e => e.Id == eventId)
            .FirstOrDefaultAsync(cancellationToken);

        if (@event == null)
        {
            throw new ResourceDoesNotExistException($"Event with the ID '{eventId}' does not exist.");
        }

        await _cache.AddAsync(eventId.ToString(), @event, cancellationToken);

        return ToSectionSeatsDTO(@event, sectionId);
    }

    private IQueryable<Persistence.Entities.Event> GetRequestEventDataQuery()
    {
        return _eventContext.Events
            .Include(e => e.Venue)
            .Include(s => s.EventManifest)
            .ThenInclude(s => s.State)
            .Include(s => s.EventManifest)
            .ThenInclude(s => s.PriceOptions)
            .Include(s => s.EventManifest)
            .ThenInclude(s => s.Seat)
            .ThenInclude(s => s.Row)
            .AsNoTracking();
    }

    private static Event ToDTO(Persistence.Entities.Event @event)
        => new Event
        {
            Id = @event.Id,
            Name = @event.Name,
            Date = @event.Date,
            VenueId = @event.VenueId,
            Address = new Address
            {
                Country = @event.Venue.Country,
                Region = @event.Venue.Region,
                City = @event.Venue.City,
                Street = @event.Venue.Street,
            }
        };

    private static IEnumerable<SectionSeat> ToSectionSeatsDTO(Persistence.Entities.Event @event, int sectionId)
    {
        return @event.EventManifest
            .Where(s => s.Seat.Row.SectionId == sectionId)
            .Select(s => new SectionSeat
            {
                SectionId = sectionId,
                RowId = s.Seat.RowId,
                SeatId = s.SeatId,
                StateId = (SeatState)s.StateId,
                StateName = s.State.Name,
                PriceOptions = s.PriceOptions.Select(o => new PriceOption
                {
                    Id = o.Id,
                    Name = o.Name,
                }).ToArray(),
            });
    }
}
