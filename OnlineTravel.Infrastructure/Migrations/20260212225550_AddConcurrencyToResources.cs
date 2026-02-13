using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConcurrencyToResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "tours",
                table: "TourSchedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "tours",
                table: "Tours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "tours",
                table: "Tours",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "hotels",
                table: "Rooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "flights",
                table: "FlightSeats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "flights",
                table: "Flights",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "flights",
                table: "Flights",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReservedAt",
                schema: "cars",
                table: "Cars",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "tours",
                table: "TourSchedules");

            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "tours",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "tours",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "hotels",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "flights",
                table: "FlightSeats");

            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "flights",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "flights",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "LastReservedAt",
                schema: "cars",
                table: "Cars");
        }
    }
}
