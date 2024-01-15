using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IWent.Api.Extensions;
using IWent.Api.Filters;
using IWent.Api.HealthChecks;
using IWent.Persistence;
using IWent.Services;
using IWent.Services.Caching;
using IWent.Services.Cart;
using IWent.Services.DTO;
using IWent.Services.Notifications;
using IWent.Services.Notifications.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        builder.Services.AddSingleton<ServiceBusClient>(services =>
        {
            var busConfiguration = services.GetRequiredService<IBusConnectionConfiguration>();
            var credential = new DefaultAzureCredential(includeInteractiveCredentials: true);
            return new ServiceBusClient(busConfiguration.Namespace, credential);
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
                return busConfiguration.QueueName;
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
