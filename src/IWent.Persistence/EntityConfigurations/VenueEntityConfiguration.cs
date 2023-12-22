using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class VenueEntityConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.Property(v => v.Name)
            .HasMaxLength(50);

        builder.Property(v => v.Country)
            .HasMaxLength(30);

        builder.Property(v => v.Region)
            .HasMaxLength(30);

        builder.Property(v => v.City)
            .HasMaxLength(30);

        builder.Property(v => v.Street)
            .HasMaxLength(100);

        builder.HasMany(v => v.Sections)
            .WithOne(s => s.Venue)
            .HasForeignKey(s => s.VenueId);
    }
}
