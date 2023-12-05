using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class EventSeatEntityConfiguration : IEntityTypeConfiguration<EventSeat>
{
    public void Configure(EntityTypeBuilder<EventSeat> builder)
    {
        builder.HasKey(s => new { s.SeatId, s.EventId });

        builder.HasOne(s => s.Event)
            .WithMany(e => e.EventManifest)
            .HasForeignKey(s => s.EventId);

        builder.HasOne(s => s.Seat)
            .WithMany(s => s.EventSeats)
            .HasForeignKey(s => s.SeatId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(s => s.State)
            .WithMany()
            .HasForeignKey(s => s.StateId);

        builder.HasMany(s => s.PriceOptions)
            .WithMany(p => p.Seats);

        builder.ToTable("events_seats");

        builder.Property(s => s.Version)
            .IsConcurrencyToken();
    }
}
