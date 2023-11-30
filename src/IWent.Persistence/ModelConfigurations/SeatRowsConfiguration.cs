using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class SeatRowsConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasOne(s => s.Price)
            .WithMany(p => p.Seats);
    }
}
