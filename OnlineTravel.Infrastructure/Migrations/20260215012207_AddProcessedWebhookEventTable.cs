using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedWebhookEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                schema: "bookings",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                schema: "bookings",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                schema: "bookings",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProcessedWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedWebhookEvents_EventId",
                table: "ProcessedWebhookEvents",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedWebhookEvents");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                schema: "bookings",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                schema: "bookings",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                schema: "bookings",
                table: "Bookings");
        }
    }
}
