using System.Linq;
using IWent.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IWent.Api.Tests.Client;

public class EventsWebApplicationFactory : WebApplicationFactory<Program>
{
    public EventsWebApplicationFactory()
    {
        ContextMock = new Mock<EventContext>(args: new DbContextOptionsBuilder().Options);
    }

    public Mock<EventContext> ContextMock { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext configuration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EventContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the mocked DbContext
            services.AddScoped(provider => ContextMock.Object);
        });
    }
}
