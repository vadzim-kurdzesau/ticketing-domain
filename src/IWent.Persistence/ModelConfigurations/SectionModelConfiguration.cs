using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class SectionModelConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(50);

        builder.HasMany(s => s.Rows)
            .WithOne(r => r.Section);
    }
}
