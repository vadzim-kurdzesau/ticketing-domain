using System.Linq;
using IWent.Api;
using IWent.Persistence;
using IWent.Persistence.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IWent.IntegrationTests.Setup;

public class IntegrationTestsApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.SeedDatabase();
        });
    }
}

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection SeedDatabase(this IServiceCollection services)
    {
        using var servicesProvider = services.BuildServiceProvider();
        using var scope = servicesProvider.CreateScope();

        var newEvent = TestData.Event;
        var priceOptions = TestData.PriceOptions;

        var dbContext = scope.ServiceProvider.GetRequiredService<EventContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        using (var transaction = dbContext.Database.BeginTransaction())
        {
            dbContext.Events.Add(newEvent);
            dbContext.Prices.AddRange(priceOptions);

            dbContext.SaveChanges();

            foreach (var seat in newEvent.Venue.Sections.SelectMany(s => s.Rows).SelectMany(r => r.Seats))
            {
                dbContext.EventSeats.Add(new EventSeat
                {
                    EventId = newEvent.Id,
                    SeatId = seat.Id,
                    StateId = SeatStatus.Available,
                    PriceOptions = priceOptions,
                });
            }

            dbContext.SaveChanges();
            transaction.Commit();
        }

        return services;
    }
}
