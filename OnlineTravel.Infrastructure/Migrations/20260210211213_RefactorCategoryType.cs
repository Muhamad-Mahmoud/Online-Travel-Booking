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
            // Safely handle column rename
            migrationBuilder.Sql(@"
                IF EXISTS(SELECT 1 FROM sys.columns 
                          WHERE Name = N'Key' AND Object_ID = Object_ID(N'[infra].[Categories]'))
                BEGIN
                    EXEC sp_rename N'[infra].[Categories].[Key]', N'Type', 'COLUMN';
                END
            ");

            // Safely handle index rename
            migrationBuilder.Sql(@"
                IF EXISTS(SELECT 1 FROM sys.indexes 
                          WHERE Name = N'IX_Categories_Key' AND Object_ID = Object_ID(N'[infra].[Categories]'))
                BEGIN
                    EXEC sp_rename N'[infra].[Categories].[IX_Categories_Key]', N'IX_Categories_Type', 'INDEX';
                END
            ");
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
