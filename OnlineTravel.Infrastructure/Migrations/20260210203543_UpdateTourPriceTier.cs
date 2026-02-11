using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourPriceTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "tours",
                table: "TourPriceTiers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "tours",
                table: "TourPriceTiers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDayCurrency",
                schema: "cars",
                table: "CarExtras");

            migrationBuilder.RenameColumn(
                name: "PricePerRentalCurrency",
                schema: "cars",
                table: "CarExtras",
                newName: "Currency");
        }
    }
}
