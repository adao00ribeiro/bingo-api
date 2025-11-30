using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCrypto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "withdrawals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "transaction_histories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "sellers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "scratch_tickets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "scratch_seller_games",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "scratch_prizes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "scratch_games",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "scratch_buys",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "rounds",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "rooms_sellers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "rooms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "amount",
                table: "recharges",
                type: "numeric(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                table: "recharges",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "destination_address",
                table: "recharges",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "discarded_at",
                table: "recharges",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "network",
                table: "recharges",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "recharges",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tx_hash",
                table: "recharges",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "punters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "prizes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "payment_methods",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "cards_winners",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "cards",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "card_buys",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "bot_configs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscardedAt",
                table: "accumulateds",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "blockchain_networks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    rpc_url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    chain_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    discarded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blockchain_networks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blockchain_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    symbol = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    decimals = table.Column<int>(type: "integer", nullable: false),
                    is_native = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    discarded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blockchain_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blockchain_token_addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token_id = table.Column<Guid>(type: "uuid", nullable: false),
                    network_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contract_address = table.Column<string>(type: "character varying(42)", unicode: false, maxLength: 42, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    discarded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blockchain_token_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_blockchain_token_addresses_network_id",
                        column: x => x.network_id,
                        principalTable: "blockchain_networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_blockchain_token_addresses_token_id",
                        column: x => x.token_id,
                        principalTable: "blockchain_tokens",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blockchain_token_addresses_network_id",
                table: "blockchain_token_addresses",
                column: "network_id");

            migrationBuilder.CreateIndex(
                name: "ux_blockchain_token_addresses_token_network",
                table: "blockchain_token_addresses",
                columns: new[] { "token_id", "network_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blockchain_token_addresses");

            migrationBuilder.DropTable(
                name: "blockchain_networks");

            migrationBuilder.DropTable(
                name: "blockchain_tokens");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "withdrawals");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "transaction_histories");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "sellers");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "scratch_tickets");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "scratch_seller_games");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "scratch_prizes");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "scratch_games");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "scratch_buys");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "rounds");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "rooms_sellers");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "destination_address",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "discarded_at",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "network",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "token",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "tx_hash",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "punters");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "prizes");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "payment_methods");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "cards_winners");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "card_buys");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "bot_configs");

            migrationBuilder.DropColumn(
                name: "DiscardedAt",
                table: "accumulateds");
        }
    }
}
