using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class SeatStateEntityConfiguration : IEntityTypeConfiguration<SeatState>
{
    public void Configure(EntityTypeBuilder<SeatState> builder)
    {
        builder.HasKey(s => s.Id);
    }
}
