using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFKsToChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_UserId",
                table: "RoomMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_AspNetUsers_UserId",
                table: "RoomMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_AspNetUsers_UserId",
                table: "RoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_UserId",
                table: "RoomMembers");
        }
    }
}
