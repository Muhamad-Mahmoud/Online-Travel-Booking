using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initahmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Key",
                schema: "infra",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MaxGuests",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Refundable",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Amenities",
                schema: "hotels",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "infra",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "hotels",
                table: "Rooms",
                newName: "PriceCurrency");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                schema: "hotels",
                table: "Rooms",
                newName: "PriceAmount");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "infra",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "infra",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "PriceCurrency",
                schema: "hotels",
                table: "Rooms",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "PriceAmount",
                schema: "hotels",
                table: "Rooms",
                newName: "BasePrice");

            migrationBuilder.AddColumn<int>(
                name: "MaxGuests",
                schema: "hotels",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Refundable",
                schema: "hotels",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                schema: "hotels",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "infra",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Key",
                schema: "infra",
                table: "Categories",
                column: "Key",
                unique: true);
        }
    }
}
