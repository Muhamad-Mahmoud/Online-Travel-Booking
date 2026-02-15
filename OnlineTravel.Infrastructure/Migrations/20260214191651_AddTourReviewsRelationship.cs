using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTravel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTourReviewsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ItemId",
                schema: "reviews",
                table: "Reviews",
                column: "ItemId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reviews_Tours_ItemId",
            //    schema: "reviews",
            //    table: "Reviews",
            //    column: "ItemId",
            //    principalSchema: "tours",
            //    principalTable: "Tours",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Reviews_Tours_ItemId",
            //    schema: "reviews",
            //    table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ItemId",
                schema: "reviews",
                table: "Reviews");
        }
    }
}
