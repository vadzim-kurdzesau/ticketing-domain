﻿using IWent.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IWent.Persistence.ModelConfigurations;

internal class EventModelConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(150);

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events);
    }
}