using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedAccommodations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Housing",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Housing_UserId",
                table: "Housing",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Housing_Users_UserId",
                table: "Housing",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Housing_Users_UserId",
                table: "Housing");

            migrationBuilder.DropIndex(
                name: "IX_Housing_UserId",
                table: "Housing");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Housing");
        }
    }
}
