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
            migrationBuilder.DropColumn(
                name: "AdultPrice",
                schema: "tours",
                table: "TourPriceTiers");

            migrationBuilder.DropColumn(
                name: "ChildPrice",
                schema: "tours",
                table: "TourPriceTiers");

            migrationBuilder.RenameColumn(
                name: "InfantPrice",
                schema: "tours",
                table: "TourPriceTiers",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "cars",
                table: "CarExtras",
                newName: "PricePerRentalCurrency");

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

            migrationBuilder.AddColumn<string>(
                name: "PricePerDayCurrency",
                schema: "cars",
                table: "CarExtras",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDayCurrency",
                schema: "cars",
                table: "CarExtras");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "tours",
                table: "TourPriceTiers",
                newName: "InfantPrice");

            migrationBuilder.RenameColumn(
                name: "PricePerRentalCurrency",
                schema: "cars",
                table: "CarExtras",
                newName: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "tours",
                table: "TourPriceTiers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "tours",
                table: "TourPriceTiers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdultPrice",
                schema: "tours",
                table: "TourPriceTiers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ChildPrice",
                schema: "tours",
                table: "TourPriceTiers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
