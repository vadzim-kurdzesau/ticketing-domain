using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Configuration;
using IWent.BookingTimer.Handling;
using IWent.BookingTimer.Extensions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IWent.BookingTimer.Handling.Timers;

namespace IWent.BookingTimer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<ServiceBusListener>();
        builder.Services.AddHostedService<MessageHandler>();

        builder.Services.AddConfiguration<IBusConfiguration, BusConfiguration>("BusConnection");
        builder.Services.AddConfiguration<ITimerConfiguration, TimerConfiguration>("Timer");
        builder.Services.AddSingleton(typeof(IBackgroundQueue<>), typeof(BackgroundQueue<>));

        var busConfiguration = builder.Services.BuildServiceProvider()
            .GetRequiredService<IBusConfiguration>();
        builder.Services.AddAzureClients(factoryBuilder =>
        {
            factoryBuilder.AddServiceBusClientWithNamespace(busConfiguration.Namespace);

            factoryBuilder.AddClient<ServiceBusReceiver, ServiceBusReceiverOptions>((options, credential, services) =>
            {
                var serviceClient = services.GetRequiredService<ServiceBusClient>();
                var configuration = services.GetRequiredService<IBusConfiguration>();
                options.ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;

                return serviceClient.CreateReceiver(configuration.ReceiverQueueName, options);
            }).WithName(Constants.ServiceBusReceiverName);

            factoryBuilder.AddClient<ServiceBusSender, ServiceBusSenderOptions>((options, credential, services) =>
            {
                var serviceClient = services.GetRequiredService<ServiceBusClient>();
                var configuration = services.GetRequiredService<IBusConfiguration>();

                return serviceClient.CreateSender(configuration.SenderQueueName, options);
            }).WithName(Constants.ServiceBusSenderName);

            factoryBuilder.UseCredential(new DefaultAzureCredential());
        });

        builder.Services.AddSingleton<ITimerFactory, TimerFactory>();

        var host = builder.Build();
        host.Run();
    }
}