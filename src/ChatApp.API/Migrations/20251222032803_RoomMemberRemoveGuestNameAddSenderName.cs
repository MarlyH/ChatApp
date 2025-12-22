using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.API.Migrations
{
    /// <inheritdoc />
    public partial class RoomMemberRemoveGuestNameAddSenderName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestName",
                table: "RoomMembers");

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "RoomMembers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "RoomMembers");

            migrationBuilder.AddColumn<string>(
                name: "GuestName",
                table: "RoomMembers",
                type: "text",
                nullable: true);
        }
    }
}
