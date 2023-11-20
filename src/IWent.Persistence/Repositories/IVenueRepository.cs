using System.Collections.Generic;
using System.Threading.Tasks;
using IWent.Persistence.Models;

namespace IWent.Persistence.Repositories;

public interface IVenueRepository
{
    Task<Venue?> GetAsync(int id);

    Task<IEnumerable<Venue>> GetAllAsync();

    Task CreateAsync(Venue newVenue);

    Task UpdateAsync(Venue update);

    Task DeleteAsync(int id);
}
