using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePriceTypesWithOnePricePropAndRemoveDisplayOrderProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "tours",
                table: "TourSchedules");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "tours",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "AdultPrice",
                schema: "tours",
                table: "TourPriceTiers");

            migrationBuilder.DropColumn(
                name: "ChildPrice",
                schema: "tours",
                table: "TourPriceTiers");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "tours",
                table: "TourPriceTiers");

            migrationBuilder.RenameColumn(
                name: "InfantPrice",
                schema: "tours",
                table: "TourPriceTiers",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "tours",
                table: "TourPriceTiers",
                newName: "InfantPrice");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "tours",
                table: "TourSchedules",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "tours",
                table: "Tours",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "tours",
                table: "TourPriceTiers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
