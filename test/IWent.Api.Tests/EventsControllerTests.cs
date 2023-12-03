using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using IWent.Api.Tests.Client;
using IWent.Services.DTO.Common;
using IWent.Services.DTO.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace IWent.Api.Tests;

public class EventsControllerTests : IClassFixture<EventsWebApplicationFactory>
{
    private readonly EventsWebApplicationFactory _webApplicationFactory;

    public EventsControllerTests(EventsWebApplicationFactory webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    [Theory]
    [InlineData(1, 10, 5)]
    public async Task GetEvents_ReturnsPaginatedAvailableEvents(int page, int size, int eventsExpected)
    {
        // Arrange
        var url = $"api/events?page={page}&size={size}";

        var existingEvents = TestData.Events.OrderBy(e => e.Date);
        _webApplicationFactory.ContextMock.Setup(c => c.Events)
            .ReturnsDbSet(existingEvents);

        var client = _webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var events = JsonConvert.DeserializeObject<List<Event>>(json);

        // Assert
        Assert.NotNull(events);

        events.Should().BeEquivalentTo(existingEvents, options => options
                .Using<Address>(ctx =>
                {
                    ctx.Subject.Country.Should().Be(ctx.Expectation.Country);
                    ctx.Subject.Street.Should().Be(ctx.Expectation.Street);
                    ctx.Subject.Region.Should().Be(ctx.Expectation.Region);
                    ctx.Subject.City.Should().Be(ctx.Expectation.City);
                })
                .WhenTypeIs<Address>()
                .ExcludingMissingMembers());

        Assert.Equal(eventsExpected, events.Count);
    }
}
