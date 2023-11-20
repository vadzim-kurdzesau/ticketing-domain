using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class VenueModelConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("dbo.venue");

        builder.HasMany(v => v.Sections)
            .WithOne(s => s.Venue)
            .HasForeignKey(s => s.VenueId);
    }
}
