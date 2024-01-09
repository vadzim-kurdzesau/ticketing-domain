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

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Row> Rows { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<EventSeat> EventSeats { get; set; }

    public virtual DbSet<SeatState> SeatStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new EventEntityConfiguration())
            .ApplyConfiguration(new VenueEntityConfiguration())
            .ApplyConfiguration(new SectionEntityConfiguration())
            .ApplyConfiguration(new RowEntityConfiguration())
            .ApplyConfiguration(new SeatEntityConfiguration())
            .ApplyConfiguration(new PriceEntityConfiguration())
            .ApplyConfiguration(new OrderItemEntityConfiguration())
            .ApplyConfiguration(new EventSeatEntityConfiguration())
            .ApplyConfiguration(new SeatStateEntityConfiguration());
    }
}
