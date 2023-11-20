using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWent.Persistence.Models;

namespace IWent.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    public Task CreateAsync(Event newEvent)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Event>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Event?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Event update)
    {
        throw new NotImplementedException();
    }
}
