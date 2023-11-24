using IWent.Persistence.Entities;
using IWent.Persistence.EntityConfigurations;
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

    public DbSet<Section> Sections { get; set; }

    public DbSet<Row> Rows { get; set; }

    public DbSet<Seat> Seats { get; set; }

    public DbSet<Price> Prices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new EventModelConfiguration())
            .ApplyConfiguration(new VenueModelConfiguration())
            .ApplyConfiguration(new SectionModelConfiguration())
            .ApplyConfiguration(new RowModelConfiguration())
            .ApplyConfiguration(new SeatRowsConfiguration())
            .ApplyConfiguration(new PriceModelConfiguration());
    }
}
