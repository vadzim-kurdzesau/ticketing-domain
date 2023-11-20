using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class EventModelConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("dbo.events");

        builder.Property(e => e.VenueId)
            .HasColumnName("venue_id");

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events);
    }
}
