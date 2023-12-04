using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IWent.Api.Parameters;
using IWent.Api.Tests.Setup;
using IWent.Services.DTO.Events;
using IWent.Tests.Shared;
using Moq.EntityFrameworkCore;
using Xunit;

namespace IWent.Api.Tests;

public class EventsControllerTests : IClassFixture<EventsWebApplicationFactory>
{
    private readonly EventsWebApplicationFactory _applicationFactory;
    private readonly ApiClient _client;

    public EventsControllerTests(EventsWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _client = new ApiClient(_applicationFactory.CreateClient());
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 2)]
    [InlineData(4, 1)]
    [InlineData(4, 3)]
    [InlineData(1, 3)]
    public async Task GetEvents_ReturnsPaginatedAvailableEvents(int page, int size)
    {
        // Arrange
        var existingEvents = TestData.Events.OrderBy(e => e.Date);
        var expectedEvents = existingEvents.Skip((page - 1) * size)
            .Take(size);
        _applicationFactory.ContextMock.Setup(c => c.Events)
            .ReturnsDbSet(existingEvents);

        // Act
        var events = await _client.GetEventsAsync(new PaginationParameters(page, size));

        // Assert
        Assert.NotNull(events);
        events.Should().BeEquivalentTo(expectedEvents, options => options
                    .WithMapping<Event>(e => e.Venue, e => e.Address)
                    .ExcludingMissingMembers());

        Assert.Equal(expectedEvents.Count(), events.Count());
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    public async Task GetSections_SeatSections(int eventId, int sectionId)
    {
        // Arrange
        var existingSeats = TestData.EventSeats;
        var expectedSeats = existingSeats.Where(s => s.EventId == eventId && s.Seat.Row.SectionId == sectionId).ToList();
        _applicationFactory.ContextMock.Setup(c => c.EventSeats)
            .ReturnsDbSet(existingSeats);

        // Act
        var seats = await _client.GetSectionSeatsAsync(eventId, sectionId);

        // Assert
        Assert.NotNull(seats);
        Assert.All(seats, s => Assert.Equal(s.SectionId, sectionId));
        seats.Should().BeEquivalentTo(expectedSeats, options => options
            .ExcludingMissingMembers());
    }
}
