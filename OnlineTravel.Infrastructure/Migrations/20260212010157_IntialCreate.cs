using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migration content removed to sync EF Core Snapshot with the actual Database state.
            // The database already contains these objects.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookingDetails",
                schema: "bookings");

            migrationBuilder.DropTable(
                name: "CarPricingTiers",
                schema: "cars");

            migrationBuilder.DropTable(
                name: "Favorites",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "FlightFares",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "FlightSeats",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "hotels");

            migrationBuilder.DropTable(
                name: "TourSchedules",
                schema: "tours");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "cars");

            migrationBuilder.DropTable(
                name: "FareRules",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "Flights",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "Bookings",
                schema: "bookings");

            migrationBuilder.DropTable(
                name: "Hotels",
                schema: "hotels");

            migrationBuilder.DropTable(
                name: "TourPriceTiers",
                schema: "tours");

            migrationBuilder.DropTable(
                name: "CarBrands",
                schema: "cars");

            migrationBuilder.DropTable(
                name: "Airports",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "Carriers",
                schema: "flights");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "Tours",
                schema: "tours");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "infra");
        }
    }
}
