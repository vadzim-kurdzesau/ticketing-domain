using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Api.Parameters;
using IWent.Services;
using IWent.Services.DTO.Events;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventsService _eventsService;

    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [HttpGet]
    [ResponseCache(Duration = 15, VaryByQueryKeys = new string[] { "page", "size" })]
    public Task<IEnumerable<Event>> GetEvents([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        return _eventsService.GetEventsAsync(parameters.Page, parameters.Size, cancellationToken);
    }

    [HttpGet("{eventId}/sections/{sectionId}/seats")]
    public Task<IEnumerable<SectionSeat>> GetSectionSeats(int eventId, int sectionId, CancellationToken cancellationToken)
    {
        return _eventsService.GetSectionSeats(eventId, sectionId, cancellationToken);
    }
}
