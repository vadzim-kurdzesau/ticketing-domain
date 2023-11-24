using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence.Entities;
using IWent.Persistence.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IWent.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly EventContext _eventContext;

    public EventRepository(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public Task CreateAsync(Event newEvent, CancellationToken cancellationToken)
    {
        _eventContext.Events.Add(newEvent);
        return _eventContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventContext.Events.FindAsync(new object[] { id }, cancellationToken);
        if (existingEvent == null)
        {
            throw new EnityDoesNotExistException($"Event with the id '{id}' does not exist.");
        }

        _eventContext.Events.Remove(existingEvent);
        await _eventContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAllAsync(Predicate<Event> filter, CancellationToken cancellationToken)
    {
        var events = await _eventContext.Events
            .Where(e => filter(e))
            .ToListAsync(cancellationToken);

        return events;
    }

    public Task<Event?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return _eventContext.Events.Where(e => e.Id == id)
            .Include(e => e.Venue)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateAsync(Event update, CancellationToken cancellationToken)
    {
        _eventContext.Events.Update(update);
        return _eventContext.SaveChangesAsync(cancellationToken);
    }
}
