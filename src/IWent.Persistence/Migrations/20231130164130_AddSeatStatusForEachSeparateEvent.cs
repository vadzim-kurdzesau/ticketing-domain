using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatStatusForEachSeparateEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_item_seats_seat_id",
                table: "order_item");

            migrationBuilder.DropTable(
                name: "price_seat");

            migrationBuilder.DropIndex(
                name: "ix_order_item_seat_id",
                table: "order_item");

            migrationBuilder.DropColumn(
                name: "state",
                table: "seats");

            migrationBuilder.AddColumn<int>(
                name: "event_id",
                table: "order_item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "events_seats",
                columns: table => new
                {
                    seat_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events_seats", x => new { x.seat_id, x.event_id });
                    table.ForeignKey(
                        name: "fk_events_seats_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_events_seats_seats_seat_id",
                        column: x => x.seat_id,
                        principalTable: "seats",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_seat_price",
                columns: table => new
                {
                    price_options_id = table.Column<int>(type: "int", nullable: false),
                    seats_seat_id = table.Column<int>(type: "int", nullable: false),
                    seats_event_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_seat_price", x => new { x.price_options_id, x.seats_seat_id, x.seats_event_id });
                    table.ForeignKey(
                        name: "fk_event_seat_price_event_seats_seats_temp_id1",
                        columns: x => new { x.seats_seat_id, x.seats_event_id },
                        principalTable: "events_seats",
                        principalColumns: new[] { "seat_id", "event_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_seat_price_prices_price_options_id",
                        column: x => x.price_options_id,
                        principalTable: "prices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_item_seat_id_event_id",
                table: "order_item",
                columns: new[] { "seat_id", "event_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_event_seat_price_seats_seat_id_seats_event_id",
                table: "event_seat_price",
                columns: new[] { "seats_seat_id", "seats_event_id" });

            migrationBuilder.CreateIndex(
                name: "ix_events_seats_event_id",
                table: "events_seats",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_item_event_seats_seat_id",
                table: "order_item",
                columns: new[] { "seat_id", "event_id" },
                principalTable: "events_seats",
                principalColumns: new[] { "seat_id", "event_id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_item_event_seats_seat_id",
                table: "order_item");

            migrationBuilder.DropTable(
                name: "event_seat_price");

            migrationBuilder.DropTable(
                name: "events_seats");

            migrationBuilder.DropIndex(
                name: "ix_order_item_seat_id_event_id",
                table: "order_item");

            migrationBuilder.DropColumn(
                name: "event_id",
                table: "order_item");

            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "price_seat",
                columns: table => new
                {
                    price_options_id = table.Column<int>(type: "int", nullable: false),
                    seats_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_seat", x => new { x.price_options_id, x.seats_id });
                    table.ForeignKey(
                        name: "fk_price_seat_prices_price_options_id",
                        column: x => x.price_options_id,
                        principalTable: "prices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_price_seat_seats_seats_id",
                        column: x => x.seats_id,
                        principalTable: "seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_item_seat_id",
                table: "order_item",
                column: "seat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_price_seat_seats_id",
                table: "price_seat",
                column: "seats_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_item_seats_seat_id",
                table: "order_item",
                column: "seat_id",
                principalTable: "seats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
