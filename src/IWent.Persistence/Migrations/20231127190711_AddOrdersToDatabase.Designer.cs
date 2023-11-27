﻿// <auto-generated />
using System;
using IWent.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IWent.Persistence.Migrations
{
    [DbContext(typeof(EventContext))]
    [Migration("20231127190711_AddOrdersToDatabase")]
    partial class AddOrdersToDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IWent.Persistence.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("name");

                    b.Property<int>("VenueId")
                        .HasColumnType("int")
                        .HasColumnName("venue_id");

                    b.HasKey("Id")
                        .HasName("pk_events");

                    b.HasIndex("VenueId")
                        .HasDatabaseName("ix_events_venue_id");

                    b.ToTable("events", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PaymentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("payment_id");

                    b.Property<int>("PriceId")
                        .HasColumnType("int")
                        .HasColumnName("price_id");

                    b.Property<int>("SeatId")
                        .HasColumnType("int")
                        .HasColumnName("seat_id");

                    b.HasKey("Id")
                        .HasName("pk_order_item");

                    b.HasIndex("PaymentId")
                        .HasDatabaseName("ix_order_item_payment_id");

                    b.HasIndex("PriceId")
                        .HasDatabaseName("ix_order_item_price_id");

                    b.HasIndex("SeatId")
                        .IsUnique()
                        .HasDatabaseName("ix_order_item_seat_id");

                    b.ToTable("order_item", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_payments");

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(14, 6)
                        .HasColumnType("decimal(14,6)")
                        .HasColumnName("amount");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_prices");

                    b.ToTable("prices", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Row", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int")
                        .HasColumnName("number");

                    b.Property<int>("SectionId")
                        .HasColumnType("int")
                        .HasColumnName("section_id");

                    b.HasKey("Id")
                        .HasName("pk_rows");

                    b.HasIndex("SectionId")
                        .HasDatabaseName("ix_rows_section_id");

                    b.ToTable("rows", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int")
                        .HasColumnName("number");

                    b.Property<int>("RowId")
                        .HasColumnType("int")
                        .HasColumnName("row_id");

                    b.Property<int>("State")
                        .HasColumnType("int")
                        .HasColumnName("state");

                    b.HasKey("Id")
                        .HasName("pk_seats");

                    b.HasIndex("RowId")
                        .HasDatabaseName("ix_seats_row_id");

                    b.ToTable("seats", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<int>("SeatType")
                        .HasColumnType("int")
                        .HasColumnName("seat_type");

                    b.Property<int>("VenueId")
                        .HasColumnType("int")
                        .HasColumnName("venue_id");

                    b.HasKey("Id")
                        .HasName("pk_sections");

                    b.HasIndex("VenueId")
                        .HasDatabaseName("ix_sections_venue_id");

                    b.ToTable("sections", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Venue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("country");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Region")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("region");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("street");

                    b.HasKey("Id")
                        .HasName("pk_venues");

                    b.ToTable("venues", (string)null);
                });

            modelBuilder.Entity("PriceSeat", b =>
                {
                    b.Property<int>("PriceOptionsId")
                        .HasColumnType("int")
                        .HasColumnName("price_options_id");

                    b.Property<int>("SeatsId")
                        .HasColumnType("int")
                        .HasColumnName("seats_id");

                    b.HasKey("PriceOptionsId", "SeatsId")
                        .HasName("pk_price_seat");

                    b.HasIndex("SeatsId")
                        .HasDatabaseName("ix_price_seat_seats_id");

                    b.ToTable("price_seat", (string)null);
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Event", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Venue", "Venue")
                        .WithMany("Events")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_events_venues_venue_id");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.OrderItem", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Payment", "Payment")
                        .WithMany("OrderItems")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_item_payments_payment_id");

                    b.HasOne("IWent.Persistence.Entities.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_item_prices_price_id");

                    b.HasOne("IWent.Persistence.Entities.Seat", "Seat")
                        .WithOne()
                        .HasForeignKey("IWent.Persistence.Entities.OrderItem", "SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_item_seats_seat_id");

                    b.Navigation("Payment");

                    b.Navigation("Price");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Row", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Section", "Section")
                        .WithMany("Rows")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rows_sections_section_id");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Seat", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Row", "Row")
                        .WithMany("Seats")
                        .HasForeignKey("RowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_seats_rows_row_id");

                    b.Navigation("Row");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Section", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Venue", "Venue")
                        .WithMany("Sections")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sections_venues_venue_id");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("PriceSeat", b =>
                {
                    b.HasOne("IWent.Persistence.Entities.Price", null)
                        .WithMany()
                        .HasForeignKey("PriceOptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_price_seat_prices_price_options_id");

                    b.HasOne("IWent.Persistence.Entities.Seat", null)
                        .WithMany()
                        .HasForeignKey("SeatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_price_seat_seats_seats_id");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Payment", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Row", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Section", b =>
                {
                    b.Navigation("Rows");
                });

            modelBuilder.Entity("IWent.Persistence.Entities.Venue", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Sections");
                });
#pragma warning restore 612, 618
        }
    }
}
