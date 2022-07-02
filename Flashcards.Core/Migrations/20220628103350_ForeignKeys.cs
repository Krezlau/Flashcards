using Microsoft.EntityFrameworkCore.Migrations;

namespace Flashcards.Core.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserDTOName",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckDTOId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_DeckDTOId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_Decks_UserDTOName",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "DeckDTOId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "UserDTOName",
                table: "Decks");

            migrationBuilder.AddColumn<int>(
                name: "DeckId",
                table: "Flashcards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Decks",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_DeckId",
                table: "Flashcards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_UserName",
                table: "Decks",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserName",
                table: "Decks",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserName",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_DeckId",
                table: "Flashcards");

            migrationBuilder.DropIndex(
                name: "IX_Decks_UserName",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "DeckId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Decks");

            migrationBuilder.AddColumn<int>(
                name: "DeckDTOId",
                table: "Flashcards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDTOName",
                table: "Decks",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_DeckDTOId",
                table: "Flashcards",
                column: "DeckDTOId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckDTOId",
                table: "Flashcards",
                column: "DeckDTOId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
