using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Api.Parameters;
using IWent.Services;
using IWent.Services.DTO.Venues;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenuesController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    public Task<IEnumerable<Venue>> GetVenues([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        return _venueService.GetVenuesAsync(parameters.Page, parameters.Size, cancellationToken);
    }

    [HttpGet("{venueId}/sections")]
    public Task<IEnumerable<VenueSection>> GetSections(int venueId, CancellationToken cancellationToken)
    {
        return _venueService.GetSectionsAsync(venueId, cancellationToken);
    }
}
