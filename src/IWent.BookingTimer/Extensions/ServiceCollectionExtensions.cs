using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IWent.BookingTimer.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration<TInterface, TImplementation>(this IServiceCollection services, string sectionName)
        where TImplementation : class, TInterface where TInterface : class
    {
        return services.AddTransient<TInterface, TImplementation>(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            return configuration.GetRequiredSection(sectionName).Get<TImplementation>()
                ?? throw new InvalidOperationException($"Unable to get the '{typeof(TImplementation)}' from configuration.");
        });
    }
}
