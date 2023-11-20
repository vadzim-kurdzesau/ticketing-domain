using IWent.Persistence.ModelConfigurations;
using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace IWent.Persistence;

public class EventContext : DbContext
{
    public EventContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventModelConfiguration());
        modelBuilder.ApplyConfiguration(new VenueModelConfiguration());
        modelBuilder.ApplyConfiguration(new SectionModelConfiguration());
        modelBuilder.ApplyConfiguration(new RowModelConfiguration());
        modelBuilder.ApplyConfiguration(new SeatRowsConfiguration());
        modelBuilder.ApplyConfiguration(new PriceModelConfiguration());
    }
}
