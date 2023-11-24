using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence.Entities;
using IWent.Persistence.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IWent.Persistence.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly EventContext _eventContext;

    public VenueRepository(EventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public Task CreateAsync(Venue newVenue, CancellationToken cancellationToken)
    {
        _eventContext.Venues.Add(newVenue);
        return _eventContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var existingVenue = await _eventContext.Venues.FindAsync(new object[] { id }, cancellationToken);
        if (existingVenue == null)
        {
            throw new EnityDoesNotExistException($"Venue with the id '{id}' does not exist.");
        }

        _eventContext.Venues.Remove(existingVenue);
        await _eventContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Venue>> GetAllAsync(Predicate<Venue> filter, CancellationToken cancellationToken)
    {
        var events = await _eventContext.Venues
            .Where(v => filter(v))
            .ToListAsync(cancellationToken);

        return events;
    }

    public Task<Venue?> GetAsync(int id, CancellationToken cancellationToken)
    {
        return _eventContext.Venues.Where(v => v.Id == id)
            .Include(v => v.Sections)
            .ThenInclude(s => s.Rows)
            .ThenInclude(r => r.Seats)
            .ThenInclude(s => s.Price)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateAsync(Venue update, CancellationToken cancellationToken)
    {
        _eventContext.Venues.Update(update);
        return _eventContext.SaveChangesAsync(cancellationToken);
    }
}
