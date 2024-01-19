using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItemKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_order_item",
                table: "order_item");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_item",
                table: "order_item",
                columns: new[] { "payment_id", "seat_id", "event_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_order_item",
                table: "order_item");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_item",
                table: "order_item",
                columns: new[] { "payment_id", "seat_id" });
        }
    }
}
