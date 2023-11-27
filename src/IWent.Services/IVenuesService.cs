using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO.Venues;

namespace IWent.Services;

/// <summary>
/// Provides functionality for API to operate with venues.
/// </summary>
public interface IVenuesService
{
    /// <summary>
    /// Gets the specified <paramref name="amount"/> of venues starting from the specified <paramref name="page"/>.
    /// </summary>
    Task<IEnumerable<Venue>> GetVenuesAsync(int page, int amount, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all sections of the venue with the specified <paramref name="venueId"/>.
    /// </summary>
    Task<IEnumerable<VenueSection>> GetSectionsAsync(int venueId, CancellationToken cancellationToken);
}
