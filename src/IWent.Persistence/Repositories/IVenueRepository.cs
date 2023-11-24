using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence.Entities;

namespace IWent.Persistence.Repositories;

public interface IVenueRepository
{
    Task<Venue?> GetAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Venue>> GetAllAsync(Predicate<Venue> filter, CancellationToken cancellationToken);

    Task CreateAsync(Venue newVenue, CancellationToken cancellationToken);

    Task UpdateAsync(Venue update, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
