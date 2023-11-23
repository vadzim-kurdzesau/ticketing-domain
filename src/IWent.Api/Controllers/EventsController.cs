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
public class EventsController : ControllerBase
{
    private readonly EventContext _eventContext;
    private readonly IMapper _mapper;

    public EventsController(EventContext eventContext, IMapper mapper)
    {
        _eventContext = eventContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<Event>> GetEvents([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        var events = await _eventContext.Events.OrderByDescending(e => e.Date)
            .Skip((parameters.Page - 1) * parameters.Size)
            .Take(parameters.Size)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Event>>(events);
    }


    [HttpGet("{eventId}/sections/{sectionId}/seats")]
    public async Task<IEnumerable<Seat>> GetSections(int eventId, int sectionId, CancellationToken cancellationToken)
    {
        var seats = await _eventContext.Seats
            .Include(s => s.Row)
            .Include(s => s.Price)
            .Where(s => s.Row.SectionId == sectionId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Seat>>(seats);
    }
}
