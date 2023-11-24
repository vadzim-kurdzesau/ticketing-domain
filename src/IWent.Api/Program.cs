using IWent.Api.Mapping;
using IWent.Persistence;
using IWent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IWent.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
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

        builder.Services.AddAutoMapper(configuration =>
        {
            configuration.AddProfile<EventMappingProfile>();
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
