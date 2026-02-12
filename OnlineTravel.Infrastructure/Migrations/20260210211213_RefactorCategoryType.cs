using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCategoryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "infra",
                table: "Categories",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Key",
                schema: "infra",
                table: "Categories",
                newName: "IX_Categories_Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "infra",
                table: "Categories",
                newName: "Key");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Type",
                schema: "infra",
                table: "Categories",
                newName: "IX_Categories_Key");
        }
    }
}
