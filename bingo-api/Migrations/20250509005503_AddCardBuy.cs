using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCardBuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CardBuyId",
                table: "Cards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "character varying(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PresenceRate",
                table: "BotConfigs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "CardBuys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    PunterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardBuys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardBuyId",
                table: "Cards",
                column: "CardBuyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardBuys_CardBuyId",
                table: "Cards",
                column: "CardBuyId",
                principalTable: "CardBuys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardBuys_CardBuyId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CardBuys");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardBuyId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardBuyId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PresenceRate",
                table: "BotConfigs");
        }
    }
}
