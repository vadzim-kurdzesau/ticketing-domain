using System.Linq;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IWent.Notifications.Email;
using IWent.Notifications.Email.Builders.Templates;
using IWent.Notifications.Email.Configuration;
using IWent.Notifications.Extensions;
using IWent.Notifications.Handling;
using IWent.Notifications.Handling.Handlers;
using IWent.Notifications.Messaging;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IWent.Notifications;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<EmailClient>();
                services.AddHostedService<NotificationsListener>();
                services.AddConfiguration<IMessageQueueConfiguration, MessageQueueConfiguration>("MessageQueue");
                services.AddConfiguration<IEmailClientConfiguration, EmailClientConfiguration>("Email");

                var queueConfiguration = services.BuildServiceProvider().GetRequiredService<IMessageQueueConfiguration>();
                services.AddAzureClients(builder =>
                {
                    builder.AddServiceBusClientWithNamespace(queueConfiguration.QueueNamespace);

                    builder.AddClient<ServiceBusReceiver, ServiceBusReceiverOptions>((options, credential, services) =>
                    {
                        var serviceClient = services.GetRequiredService<ServiceBusClient>();
                        var configuration = services.GetRequiredService<IMessageQueueConfiguration>();
                        options.ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;

                        return serviceClient.CreateReceiver(configuration.QueueName, options);
                    })
                    .WithName(queueConfiguration.QueueName);

                    builder.UseCredential(new DefaultAzureCredential());
                });

                // Register client to authenticate in on application start
                services.AddSingleton<IEmailClient>(services => services.GetServices<IHostedService>().OfType<EmailClient>().First());
                services.AddTransient<INotificationHandlersFactory, NotificationHandlersFactory>();
                services.AddTransient<CheckoutNotificationHandler>();
                services.AddSingleton<IEmailTemplatesStorage, EmailTemplatesStorage>();
            })
            .Build();

        host.Run();
    }
}