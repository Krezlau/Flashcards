using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flashcards.Core.Migrations
{
    public partial class DeckActivityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinutesSpentLearning",
                table: "DailyActivity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeckActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedFlashcardsCount = table.Column<int>(type: "int", nullable: false),
                    MinutesSpentLearning = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckActivity_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckActivity_DeckId",
                table: "DeckActivity",
                column: "DeckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckActivity");

            migrationBuilder.DropColumn(
                name: "MinutesSpentLearning",
                table: "DailyActivity");
        }
    }
}
