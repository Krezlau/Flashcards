using Microsoft.EntityFrameworkCore.Migrations;

namespace Flashcards.Core.Migrations
{
    public partial class UserIdDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserDTOId",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Decks_UserDTOId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserDTOId",
                table: "Decks");

            migrationBuilder.AddColumn<string>(
                name: "UserDTOName",
                table: "Decks",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserDTOName",
                table: "Decks",
                column: "UserDTOName");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserDTOName",
                table: "Decks",
                column: "UserDTOName",
                principalTable: "Users",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserDTOName",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Decks_UserDTOName",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "UserDTOName",
                table: "Decks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserDTOId",
                table: "Decks",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserDTOId",
                table: "Decks",
                column: "UserDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserDTOId",
                table: "Decks",
                column: "UserDTOId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
