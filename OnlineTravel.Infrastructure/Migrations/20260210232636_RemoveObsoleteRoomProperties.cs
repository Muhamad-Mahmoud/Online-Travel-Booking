using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveObsoleteRoomProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableDatesJson",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "hotels",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "AvailableDatesJson",
                schema: "hotels",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                schema: "hotels",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
