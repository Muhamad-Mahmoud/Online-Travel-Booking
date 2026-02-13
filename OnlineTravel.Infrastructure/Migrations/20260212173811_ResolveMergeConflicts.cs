using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ResolveMergeConflicts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF OBJECT_ID(N'[cars].[CarExtras]', N'U') IS NOT NULL DROP TABLE [cars].[CarExtras];");
            migrationBuilder.Sql("IF OBJECT_ID(N'[CarExtra]', N'U') IS NOT NULL DROP TABLE [CarExtra];");

            migrationBuilder.Sql(@"
                IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'AdultPrice' AND Object_ID = Object_ID(N'[tours].[TourPriceTiers]'))
                BEGIN
                    ALTER TABLE [tours].[TourPriceTiers] DROP COLUMN [AdultPrice];
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ChildPrice' AND Object_ID = Object_ID(N'[tours].[TourPriceTiers]'))
                BEGIN
                    ALTER TABLE [tours].[TourPriceTiers] DROP COLUMN [ChildPrice];
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'InfantPrice' AND Object_ID = Object_ID(N'[tours].[TourPriceTiers]'))
                BEGIN
                    EXEC sp_rename N'[tours].[TourPriceTiers].[InfantPrice]', N'Price', 'COLUMN';
                END
            ");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "tours",
                table: "TourPriceTiers",
                newName: "InfantPrice");

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

            migrationBuilder.CreateTable(
                name: "CarExtra",
                columns: table => new
                {
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TempId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_CarExtra_TempId1", x => x.TempId1);
                    table.UniqueConstraint("AK_CarExtra_TempId2", x => x.TempId2);
                    table.ForeignKey(
                        name: "FK_CarExtra_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "cars",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarExtras",
                schema: "cars",
                columns: table => new
                {
                    CarExtraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerRental = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarExtras", x => x.CarExtraId);
                    table.ForeignKey(
                        name: "FK_CarExtras_CarExtra_CarExtraId",
                        column: x => x.CarExtraId,
                        principalTable: "CarExtra",
                        principalColumn: "TempId1",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
