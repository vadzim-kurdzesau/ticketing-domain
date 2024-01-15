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
    private readonly IVenuesService _venueService;

    public VenuesController(IVenuesService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    public Task<IEnumerable<Venue>> GetVenuesAsync([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        return _venueService.GetVenuesAsync(parameters.Page, parameters.Size, cancellationToken);
    }

    [HttpGet("{venueId}/sections")]
    public Task<IEnumerable<VenueSection>> GetSectionsAsync(int venueId, CancellationToken cancellationToken)
    {
        return _venueService.GetSectionsAsync(venueId, cancellationToken);
    }
}
