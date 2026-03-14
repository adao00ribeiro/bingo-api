using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringScratch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_scratch_prizes_scratch_games_scratch_game_id",
                table: "scratch_prizes");

            migrationBuilder.DropForeignKey(
                name: "FK_scratch_tickets_scratch_prizes_scratch_prize_id",
                table: "scratch_tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_scratch_tickets_scratch_seller_games_scratch_seller_game_id",
                table: "scratch_tickets");

            migrationBuilder.DropTable(
                name: "scratch_seller_games");

            migrationBuilder.DropIndex(
                name: "IX_scratch_tickets_scratch_prize_id",
                table: "scratch_tickets");

            migrationBuilder.DropIndex(
                name: "IX_scratch_tickets_scratch_seller_game_id",
                table: "scratch_tickets");

            migrationBuilder.DropIndex(
                name: "IX_scratch_prizes_scratch_game_id",
                table: "scratch_prizes");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "prize_won",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "revealed",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "scratch_prize_id",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "scratch_seller_game_id",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "description",
                table: "scratch_prizes");

            migrationBuilder.DropColumn(
                name: "scratch_game_id",
                table: "scratch_prizes");

            migrationBuilder.DropColumn(
                name: "layout_type",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "max_prize",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "price",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "probability",
                table: "scratch_games");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "scratch_prizes",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "seller_game_id",
                table: "scratch_buys",
                newName: "scratch_game_override_id");

            migrationBuilder.AddColumn<Guid>(
                name: "ScratchBuyId",
                table: "transaction_histories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "scratch_buy_id",
                table: "scratch_tickets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "value",
                table: "scratch_tickets",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "cols",
                table: "scratch_games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "component",
                table: "scratch_games",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "quantity_to_award",
                table: "scratch_games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rows",
                table: "scratch_games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rtp",
                table: "scratch_games",
                type: "numeric(10,4)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "value",
                table: "scratch_buys",
                type: "numeric(15,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "scratch_game_overrides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    subtitle = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    card_value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    online_house_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scratch_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DiscardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scratch_game_overrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scratch_game_overrides_online_houses_online_house_id",
                        column: x => x.online_house_id,
                        principalTable: "online_houses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scratch_game_overrides_scratch_games_scratch_game_id",
                        column: x => x.scratch_game_id,
                        principalTable: "scratch_games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transaction_histories_ScratchBuyId",
                table: "transaction_histories",
                column: "ScratchBuyId");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_attributes",
                table: "scratch_tickets",
                column: "attributes")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_prizes_scratch_ticket_id",
                table: "scratch_prizes",
                column: "scratch_ticket_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scratch_buys_punter_id",
                table: "scratch_buys",
                column: "punter_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_buys_scratch_game_override_id",
                table: "scratch_buys",
                column: "scratch_game_override_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_game_overrides_online_house_id_scratch_game_id",
                table: "scratch_game_overrides",
                columns: new[] { "online_house_id", "scratch_game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scratch_game_overrides_scratch_game_id",
                table: "scratch_game_overrides",
                column: "scratch_game_id");

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_buys_punters_punter_id",
                table: "scratch_buys",
                column: "punter_id",
                principalTable: "punters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_buys_scratch_game_overrides_scratch_game_override_id",
                table: "scratch_buys",
                column: "scratch_game_override_id",
                principalTable: "scratch_game_overrides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_prizes_scratch_tickets_scratch_ticket_id",
                table: "scratch_prizes",
                column: "scratch_ticket_id",
                principalTable: "scratch_tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_histories_scratch_buys_ScratchBuyId",
                table: "transaction_histories",
                column: "ScratchBuyId",
                principalTable: "scratch_buys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_scratch_buys_punters_punter_id",
                table: "scratch_buys");

            migrationBuilder.DropForeignKey(
                name: "FK_scratch_buys_scratch_game_overrides_scratch_game_override_id",
                table: "scratch_buys");

            migrationBuilder.DropForeignKey(
                name: "FK_scratch_prizes_scratch_tickets_scratch_ticket_id",
                table: "scratch_prizes");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_histories_scratch_buys_ScratchBuyId",
                table: "transaction_histories");

            migrationBuilder.DropTable(
                name: "scratch_game_overrides");

            migrationBuilder.DropIndex(
                name: "IX_transaction_histories_ScratchBuyId",
                table: "transaction_histories");

            migrationBuilder.DropIndex(
                name: "IX_scratch_tickets_attributes",
                table: "scratch_tickets");

            migrationBuilder.DropIndex(
                name: "IX_scratch_prizes_scratch_ticket_id",
                table: "scratch_prizes");

            migrationBuilder.DropIndex(
                name: "IX_scratch_buys_punter_id",
                table: "scratch_buys");

            migrationBuilder.DropIndex(
                name: "IX_scratch_buys_scratch_game_override_id",
                table: "scratch_buys");

            migrationBuilder.DropColumn(
                name: "ScratchBuyId",
                table: "transaction_histories");

            migrationBuilder.DropColumn(
                name: "value",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "cols",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "component",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "quantity_to_award",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "rows",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "rtp",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "value",
                table: "scratch_buys");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "scratch_prizes",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "scratch_game_override_id",
                table: "scratch_buys",
                newName: "seller_game_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "scratch_buy_id",
                table: "scratch_tickets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "multiplier",
                table: "scratch_tickets",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "prize_won",
                table: "scratch_tickets",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "revealed",
                table: "scratch_tickets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "scratch_prize_id",
                table: "scratch_tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "scratch_seller_game_id",
                table: "scratch_tickets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "scratch_prizes",
                type: "character varying(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "scratch_game_id",
                table: "scratch_prizes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "layout_type",
                table: "scratch_games",
                type: "integer",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "max_prize",
                table: "scratch_games",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "scratch_games",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "probability",
                table: "scratch_games",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "scratch_seller_games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    scratch_game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DiscardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_scratch_prize_id",
                table: "scratch_tickets",
                column: "scratch_prize_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scratch_tickets_scratch_seller_game_id",
                table: "scratch_tickets",
                column: "scratch_seller_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_prizes_scratch_game_id",
                table: "scratch_prizes",
                column: "scratch_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_scratch_game_id",
                table: "scratch_seller_games",
                column: "scratch_game_id");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_seller_id_scratch_game_id",
                table: "scratch_seller_games",
                columns: new[] { "seller_id", "scratch_game_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_prizes_scratch_games_scratch_game_id",
                table: "scratch_prizes",
                column: "scratch_game_id",
                principalTable: "scratch_games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_tickets_scratch_prizes_scratch_prize_id",
                table: "scratch_tickets",
                column: "scratch_prize_id",
                principalTable: "scratch_prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_scratch_tickets_scratch_seller_games_scratch_seller_game_id",
                table: "scratch_tickets",
                column: "scratch_seller_game_id",
                principalTable: "scratch_seller_games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
