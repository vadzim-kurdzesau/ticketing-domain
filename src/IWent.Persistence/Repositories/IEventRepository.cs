using System.Collections.Generic;
using System.Threading.Tasks;
using IWent.Persistence.Models;

namespace IWent.Persistence.Repositories;

public interface IEventRepository
{
    Task<Event?> GetAsync(int id);

    Task<IEnumerable<Event>> GetAllAsync();

    Task CreateAsync(Event newEvent);

    Task UpdateAsync(Event update);

    Task DeleteAsync(int id);
}
