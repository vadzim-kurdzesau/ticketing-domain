using System.Linq;
using IWent.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IWent.Api.Tests.Setup;

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
            services.ReplaceService(ContextMock.Object);
        });
    }
}

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceService<TService>(this IServiceCollection services, TService newImplementation) where TService : class
    {
        var existingDescriptor = services.Single(d => d.ServiceType == typeof(TService));
        services.Remove(existingDescriptor);

        services.Add(new ServiceDescriptor(typeof(TService), (services) => newImplementation, existingDescriptor.Lifetime));
        return services;
    }
}
