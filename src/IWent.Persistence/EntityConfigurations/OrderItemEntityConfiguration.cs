﻿using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.EntityConfigurations;

internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasOne(i => i.Payment)
            .WithMany(p => p.OrderItems);

        builder.HasOne(i => i.Seat)
            .WithOne();

        builder.HasOne(i => i.Price)
            .WithMany();
    }
}