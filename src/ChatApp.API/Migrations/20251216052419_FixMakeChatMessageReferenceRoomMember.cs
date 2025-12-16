using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.API.Migrations
{
    /// <inheritdoc />
    public partial class FixMakeChatMessageReferenceRoomMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "SenderId",
                table: "ChatMessages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_RoomMembers_SenderId",
                table: "ChatMessages",
                column: "SenderId",
                principalTable: "RoomMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_RoomMembers_SenderId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "SenderId",
                table: "ChatMessages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "ChatMessages",
                type: "text",
                nullable: true);
        }
    }
}
