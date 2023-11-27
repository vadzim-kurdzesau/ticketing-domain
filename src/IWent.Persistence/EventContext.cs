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

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new EventEntityConfiguration())
            .ApplyConfiguration(new VenueEntityConfiguration())
            .ApplyConfiguration(new SectionEntityConfiguration())
            .ApplyConfiguration(new RowEntityConfiguration())
            .ApplyConfiguration(new SeatEntityConfiguration())
            .ApplyConfiguration(new PriceEntityConfiguration())
            .ApplyConfiguration(new OrderItemEntityConfiguration());
    }
}
