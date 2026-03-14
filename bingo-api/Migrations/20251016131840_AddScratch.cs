using System;
using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Structs.Scratchcard;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddScratch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scratch_buys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    seller_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    punter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_buys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scratch_games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    layout_type = table.Column<int>(type: "integer", maxLength: 10, nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    max_prize = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    probability = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    attributes = table.Column<ScratchGameAttributes>(type: "jsonb", nullable: false),
                    allowed_multipliers = table.Column<int[]>(type: "integer[]", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scratch_prizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    scratch_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scratch_ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_prizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scratch_prizes_scratch_games_scratch_game_id",
                        column: x => x.scratch_game_id,
                        principalTable: "scratch_games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scratch_seller_games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scratch_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_seller_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scratch_seller_games_scratch_games_scratch_game_id",
                        column: x => x.scratch_game_id,
                        principalTable: "scratch_games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scratch_seller_games_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scratch_tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    multiplier = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    prize_won = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    revealed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    attributes = table.Column<ScratchTicketAttributes>(type: "jsonb", nullable: false),
                    scratch_seller_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scratch_prize_id = table.Column<Guid>(type: "uuid", nullable: true),
                    scratch_buy_id = table.Column<Guid>(type: "uuid", nullable: true),
                    PunterId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scratch_tickets_punters_PunterId",
                        column: x => x.PunterId,
                        principalTable: "punters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_scratch_tickets_scratch_buys_scratch_buy_id",
                        column: x => x.scratch_buy_id,
                        principalTable: "scratch_buys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_scratch_tickets_scratch_prizes_scratch_prize_id",
                        column: x => x.scratch_prize_id,
                        principalTable: "scratch_prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_scratch_tickets_scratch_seller_games_scratch_seller_game_id",
                        column: x => x.scratch_seller_game_id,
                        principalTable: "scratch_seller_games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_scratch_prizes_scratch_game_id",
                table: "scratch_prizes",
                column: "scratch_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_scratch_game_id",
                table: "scratch_seller_games",
                column: "scratch_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_seller_id",
                table: "scratch_seller_games",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_PunterId",
                table: "scratch_tickets",
                column: "PunterId");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_scratch_buy_id",
                table: "scratch_tickets",
                column: "scratch_buy_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_scratch_prize_id",
                table: "scratch_tickets",
                column: "scratch_prize_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_scratch_seller_game_id",
                table: "scratch_tickets",
                column: "scratch_seller_game_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scratch_tickets");

            migrationBuilder.DropTable(
                name: "scratch_buys");

            migrationBuilder.DropTable(
                name: "scratch_prizes");

            migrationBuilder.DropTable(
                name: "scratch_seller_games");

            migrationBuilder.DropTable(
                name: "scratch_games");
        }
    }
}
