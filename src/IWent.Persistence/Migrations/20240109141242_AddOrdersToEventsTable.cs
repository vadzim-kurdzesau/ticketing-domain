using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersToEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_order_item_event_id",
                table: "order_item",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_item_events_event_id",
                table: "order_item",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_item_events_event_id",
                table: "order_item");

            migrationBuilder.DropIndex(
                name: "ix_order_item_event_id",
                table: "order_item");
        }
    }
}
