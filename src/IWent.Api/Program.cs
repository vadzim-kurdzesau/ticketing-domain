using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IWent.Api.Filters;
using IWent.Api.HealthChecks;
using IWent.Persistence;
using IWent.Services;
using IWent.Services.Caching;
using IWent.Services.Cart;
using IWent.Services.Constants;
using IWent.Services.DTO;
using IWent.Services.Notifications;
using IWent.Services.Notifications.Configuration;
using IWent.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;

namespace IWent.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // TODO: make perfomance test to chech if Context Pooling will work here better
        builder.Services.AddDbContext<EventContext>((services, options) =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            options.UseSqlServer(configuration.GetConnectionString("Default"))
                .UseSnakeCaseNamingConvention();
        });

        builder.Services.AddScoped<IVenuesService, VenuesService>();
        builder.Services.AddScoped<IEventsService, EventsService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddSingleton<ICartStorage, InMemoryCartStorage>();
        builder.Services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));

        builder.Services.AddConfiguration<ICacheConfiguration, CacheConfiguration>("Caching");
        builder.Services.AddConfiguration<IBusConnectionConfiguration, BusConnectionConfiguration>("BusConnection");

        builder.Services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("Cache");
            options.SchemaName = "dbo";
            options.TableName = "EventsCache";
        });

        var busConfiguration = builder.Services.BuildServiceProvider()
            .GetRequiredService<IBusConnectionConfiguration>();
        builder.Services.AddAzureClients(factoryBuilder =>
        {
            factoryBuilder.AddServiceBusClientWithNamespace(busConfiguration.Namespace);

            factoryBuilder.AddClient<ServiceBusReceiver, ServiceBusReceiverOptions>((options, credential, services) =>
            {
                var serviceClient = services.GetRequiredService<ServiceBusClient>();
                var configuration = services.GetRequiredService<IBusConnectionConfiguration>();
                options.ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;

                return serviceClient.CreateReceiver(configuration.ExpiredTimersQueueName, options);
            }).WithName(ServiceBusClientNames.ExpiredTimersReceiver);

            factoryBuilder.AddClient<ServiceBusSender, ServiceBusSenderOptions>((options, credential, services) =>
            {
                var serviceClient = services.GetRequiredService<ServiceBusClient>();
                var configuration = services.GetRequiredService<IBusConnectionConfiguration>();

                return serviceClient.CreateSender(configuration.NotificationsQueueName, options);
            }).WithName(ServiceBusClientNames.NotificationsSender);

            factoryBuilder.UseCredential(
                new DefaultAzureCredential(includeInteractiveCredentials: builder.Environment.IsDevelopment()));
        });

        builder.Services.AddSingleton<INotificationClient, NotificationClient>();

        builder.Services.AddResponseCaching();
        builder.Services.AddHealthChecks()
            .AddCheck<AvailabilityCheck>("Availability")
            .AddDbContextCheck<EventContext>("Database Availability")
            .AddAzureServiceBusQueue(fullyQualifiedNamespaceFactory: services =>
            {
                var busConfiguration = services.GetRequiredService<IBusConnectionConfiguration>();
                return busConfiguration.Namespace;
            }, queueNameFactory: services =>
            {
                var busConfiguration = services.GetRequiredService<IBusConnectionConfiguration>();
                return busConfiguration.NotificationsQueueName;
            }, tokenCredentialFactory: _ => new DefaultAzureCredential(), name: "Service Bus Availability");

        var app = builder.Build();

        app.MapHealthChecks("/health");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseResponseCaching();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
