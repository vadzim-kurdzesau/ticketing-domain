using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class SectionEntityConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(50);

        builder.HasMany(s => s.Rows)
            .WithOne(r => r.Section);
    }
}
