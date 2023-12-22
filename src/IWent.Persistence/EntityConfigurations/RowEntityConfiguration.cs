using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class RowEntityConfiguration : IEntityTypeConfiguration<Row>
{
    public void Configure(EntityTypeBuilder<Row> builder)
    {
        builder.HasMany(r => r.Seats)
            .WithOne(r => r.Row);
    }
}
