using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IWent.Api.Models;
using IWent.Api.Parameters;
using IWent.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly EventContext _eventContext;
    private readonly IMapper _mapper;

    public VenuesController(EventContext eventContext, IMapper mapper)
    {
        _eventContext = eventContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<Venue>> GetVenues([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        var venues = await _eventContext.Venues.OrderBy(v => v.Name)
            .Skip((parameters.Page - 1) * parameters.Size)
            .Take(parameters.Size)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Venue>>(venues);
    }

    [HttpGet("{venueId}/sections")]
    public async Task<IEnumerable<Section>> GetSections(int venueId, CancellationToken cancellationToken)
    {
        var sections = await _eventContext.Sections
            .Where(s => s.VenueId == venueId)
            .Include(s => s.Rows)
            .ThenInclude(r => r.Seats)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Section>>(sections);
    }
}
