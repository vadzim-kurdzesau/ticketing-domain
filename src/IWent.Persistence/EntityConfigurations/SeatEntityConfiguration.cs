﻿using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class SeatEntityConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasMany(s => s.EventSeats)
            .WithOne(s => s.Seat)
            .HasForeignKey(s => s.SeatId);
    }
}
