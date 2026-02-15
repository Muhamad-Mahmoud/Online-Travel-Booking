using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultipleReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Reviews",
                columns: new[] { "UserId", "CategoryId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Reviews",
                columns: new[] { "UserId", "CategoryId", "ItemId" },
                unique: true);
        }
    }
}
