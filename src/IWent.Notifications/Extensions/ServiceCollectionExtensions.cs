using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IWent.Notifications.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration<TService, TImplementation>(this IServiceCollection services, string sectionName)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddTransient<TService, TImplementation>(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            return configuration.GetRequiredSection(sectionName).Get<TImplementation>()
                ?? throw new InvalidOperationException($"Unable to get the '{typeof(TImplementation)}' from configuration.");
        });
    }
}
