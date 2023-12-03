using System.Collections.Generic;
using System.Linq;
using IWent.Api;
using IWent.Persistence;
using IWent.Persistence.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IWent.IntegrationTests;

public class IntegrationTestsApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var servicesProvider = services.BuildServiceProvider();
            using (var scope = servicesProvider.CreateScope())
            {
                var newEvent = TestData.Event;
                var priceOptions = TestData.PriceOptions;

                var dbContext = scope.ServiceProvider.GetRequiredService<EventContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                dbContext.Events.Add(newEvent);
                dbContext.Prices.AddRange(priceOptions);
                dbContext.SaveChanges();

                var eventSeats = new List<EventSeat>();
                foreach (var seat in newEvent.Venue.Sections
                    .SelectMany(s => s.Rows)
                    .SelectMany(r => r.Seats))
                {
                    eventSeats.Add(new EventSeat
                    {
                        EventId = newEvent.Id,
                        SeatId = seat.Id,
                        StateId = SeatStatus.Available,
                        PriceOptions = priceOptions,
                    });
                }

                dbContext.EventSeats.AddRange(eventSeats);
                dbContext.SaveChanges();
            }
        });
    }
}
