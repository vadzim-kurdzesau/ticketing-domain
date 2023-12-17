using IWent.Notifications.Email;
using IWent.Notifications.Email.Builders.Templates;
using IWent.Notifications.Email.Configuration;
using IWent.Notifications.Extensions;
using IWent.Notifications.Handling;
using IWent.Notifications.Handling.Handlers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(IWent.Notifications.Startup))]

namespace IWent.Notifications;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddLogging(b => b.AddConsole());

        builder.Services.AddConfiguration<IEmailClientConfiguration, EmailClientConfiguration>("Email");

        builder.Services.AddTransient<IEmailClient, EmailClient>();
        builder.Services.AddSingleton<INotificationHandlersFactory, NotificationHandlersFactory>();
        builder.Services.AddTransient<CheckoutNotificationHandler>();
        builder.Services.AddSingleton<IEmailTemplatesStorage, EmailTemplatesStorage>();
    }
}
