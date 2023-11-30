using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class EventEntityConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(150);

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events);

        builder.HasMany(e => e.EventManifest)
            .WithOne(s => s.Event)
            .HasForeignKey(s => s.EventId);
    }
}
