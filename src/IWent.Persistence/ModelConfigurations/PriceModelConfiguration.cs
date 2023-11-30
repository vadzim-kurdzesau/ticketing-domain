using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class PriceModelConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.Property(p => p.Amount)
            .HasPrecision(precision: 14, scale: 6);
    }
}
