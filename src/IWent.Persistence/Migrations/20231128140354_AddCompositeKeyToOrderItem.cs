using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeKeyToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_order_item",
                table: "order_item");

            migrationBuilder.DropIndex(
                name: "ix_order_item_payment_id",
                table: "order_item");

            migrationBuilder.DropColumn(
                name: "id",
                table: "order_item");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_item",
                table: "order_item",
                columns: new[] { "payment_id", "seat_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_order_item",
                table: "order_item");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "order_item",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_item",
                table: "order_item",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_payment_id",
                table: "order_item",
                column: "payment_id");
        }
    }
}
