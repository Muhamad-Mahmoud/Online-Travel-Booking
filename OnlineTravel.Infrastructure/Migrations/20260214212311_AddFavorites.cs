using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Categories_CategoryId",
                schema: "reviews",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_CategoryId",
                schema: "reviews",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "reviews",
                table: "Favorites");

            migrationBuilder.RenameTable(
                name: "Favorites",
                schema: "reviews",
                newName: "Favorites");

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Favorites",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_ItemId_ItemType",
                table: "Favorites",
                columns: new[] { "UserId", "ItemId", "ItemType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId_ItemId_ItemType",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Favorites");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "Favorites",
                newSchema: "reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "reviews",
                table: "Favorites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CategoryId",
                schema: "reviews",
                table: "Favorites",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_CategoryId_ItemId",
                schema: "reviews",
                table: "Favorites",
                columns: new[] { "UserId", "CategoryId", "ItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Categories_CategoryId",
                schema: "reviews",
                table: "Favorites",
                column: "CategoryId",
                principalSchema: "infra",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
