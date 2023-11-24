using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence.Entities;

namespace IWent.Persistence.Repositories;

public interface IEventRepository
{
    Task<Event?> GetAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Event>> GetAllAsync(Predicate<Event> filter, CancellationToken cancellationToken);

    Task CreateAsync(Event newEvent, CancellationToken cancellationToken);

    Task UpdateAsync(Event update, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
