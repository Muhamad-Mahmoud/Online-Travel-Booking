using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixHotelCoordinatesAndContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInfo_Email",
                table: "Hotels",
                newName: "ContactEmail");

            migrationBuilder.AlterColumn<string>(
                name: "Address_FullAddress",
                table: "Hotels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Hotels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "ContactEmail",
                table: "Hotels",
                newName: "ContactInfo_Email");

            migrationBuilder.AlterColumn<string>(
                name: "Address_FullAddress",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
