using System;
using IWent.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatStateToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "state",
                table: "events_seats",
                newName: "state_id");

            migrationBuilder.CreateTable(
                name: "seat_states",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seat_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_seats_state_id",
                table: "events_seats",
                column: "state_id");

            migrationBuilder.AddForeignKey(
                name: "fk_events_seats_seat_states_state_id",
                table: "events_seats",
                column: "state_id",
                principalTable: "seat_states",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            foreach (var state in Enum.GetValues<SeatStatus>())
            {
                migrationBuilder.InsertData(
                    table: "seat_states",
                    columns: new[] { "id", "name" },
                    values: new object[] { (int)state, state.ToString() });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_events_seats_seat_states_state_id",
                table: "events_seats");

            migrationBuilder.DropTable(
                name: "seat_states");

            migrationBuilder.DropIndex(
                name: "ix_events_seats_state_id",
                table: "events_seats");

            migrationBuilder.RenameColumn(
                name: "state_id",
                table: "events_seats",
                newName: "state");
        }
    }
}
