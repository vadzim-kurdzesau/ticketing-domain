﻿using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class RowModelConfiguration : IEntityTypeConfiguration<Row>
{
    public void Configure(EntityTypeBuilder<Row> builder)
    {
        builder.HasMany(r => r.Seats)
            .WithOne(r => r.Row);
    }
}