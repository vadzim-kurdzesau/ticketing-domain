using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeSeatToPriceManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_seats_prices_price_id",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "ix_seats_price_id",
                table: "seats");

            migrationBuilder.DropColumn(
                name: "price_id",
                table: "seats");

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
                name: "ix_price_seat_seats_id",
                table: "price_seat",
                column: "seats_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "price_seat");

            migrationBuilder.AddColumn<int>(
                name: "price_id",
                table: "seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_seats_price_id",
                table: "seats",
                column: "price_id");

            migrationBuilder.AddForeignKey(
                name: "fk_seats_prices_price_id",
                table: "seats",
                column: "price_id",
                principalTable: "prices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
