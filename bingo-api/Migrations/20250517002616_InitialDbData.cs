using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Structs;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "card_buys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    round_id = table.Column<Guid>(type: "uuid", nullable: false),
                    punter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_buys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sellers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    PrizeBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    email = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    date_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comission = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sellers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    previous_balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    current_balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_histories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "punters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    prize_balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    is_bot = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    date_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_punters", x => x.id);
                    table.ForeignKey(
                        name: "FK_punters_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_sellers_owner_id",
                        column: x => x.owner_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recharges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    qrcode = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    imagem_qrcode = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    punter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recharges", x => x.id);
                    table.ForeignKey(
                        name: "FK_recharges_punters_punter_id",
                        column: x => x.punter_id,
                        principalTable: "punters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accumulateds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activated = table.Column<bool>(type: "boolean", nullable: false),
                    minimum_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    maximum_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    current_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    maximum_number_of_balls = table.Column<int>(type: "integer", nullable: false),
                    cumulative_percentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    increment_ball_cumulative = table.Column<bool>(type: "boolean", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accumulateds", x => x.id);
                    table.ForeignKey(
                        name: "fk_accumulated_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bot_configs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    presence_rate = table.Column<double>(type: "double precision", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bot_configs", x => x.id);
                    table.ForeignKey(
                        name: "fk_bot_config_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms_sellers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_by = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms_sellers", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_sellers_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rooms_sellers_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rounds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    numbers = table.Column<int[]>(type: "integer[]", nullable: false),
                    card_sale_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    time_between_balls = table.Column<int>(type: "integer", nullable: false, defaultValue: 4),
                    max_balls = table.Column<int>(type: "integer", nullable: false, defaultValue: 90),
                    card_rows = table.Column<int>(type: "integer", nullable: false),
                    card_columns = table.Column<int>(type: "integer", nullable: false),
                    timeline = table.Column<List<TimelineEvent>>(type: "jsonb", nullable: false),
                    started = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    finished = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rounds", x => x.id);
                    table.ForeignKey(
                        name: "FK_rounds_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    numbers = table.Column<int[]>(type: "integer[]", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    round_id = table.Column<Guid>(type: "uuid", nullable: false),
                    punter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    card_buy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.id);
                    table.ForeignKey(
                        name: "fk_card_card_buy_id",
                        column: x => x.card_buy_id,
                        principalTable: "card_buys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_card_punter_id",
                        column: x => x.punter_id,
                        principalTable: "punters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_card_round_id",
                        column: x => x.round_id,
                        principalTable: "rounds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prizes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    round_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prizes", x => x.id);
                    table.ForeignKey(
                        name: "fk_prize_round_id",
                        column: x => x.round_id,
                        principalTable: "rounds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cards_winners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    card_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prize_id = table.Column<Guid>(type: "uuid", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards_winners", x => x.id);
                    table.ForeignKey(
                        name: "fk_card_winner_card_id",
                        column: x => x.card_id,
                        principalTable: "cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_card_winner_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accumulateds_room_id",
                table: "accumulateds",
                column: "room_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bot_configs_room_id",
                table: "bot_configs",
                column: "room_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cards_card_buy_id",
                table: "cards",
                column: "card_buy_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_punter_id",
                table: "cards",
                column: "punter_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_round_id",
                table: "cards",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_winners_card_id",
                table: "cards_winners",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_winners_prize_id",
                table: "cards_winners",
                column: "prize_id");

            migrationBuilder.CreateIndex(
                name: "IX_prizes_round_id",
                table: "prizes",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "IX_punters_seller_id",
                table: "punters",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "ux_punters_email",
                table: "punters",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recharges_punter_id",
                table: "recharges",
                column: "punter_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_owner_id",
                table: "rooms",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_sellers_room_id",
                table: "rooms_sellers",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_sellers_seller_id",
                table: "rooms_sellers",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_rounds_room_id",
                table: "rounds",
                column: "room_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accumulateds");

            migrationBuilder.DropTable(
                name: "bot_configs");

            migrationBuilder.DropTable(
                name: "cards_winners");

            migrationBuilder.DropTable(
                name: "recharges");

            migrationBuilder.DropTable(
                name: "rooms_sellers");

            migrationBuilder.DropTable(
                name: "transaction_histories");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "prizes");

            migrationBuilder.DropTable(
                name: "card_buys");

            migrationBuilder.DropTable(
                name: "punters");

            migrationBuilder.DropTable(
                name: "rounds");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "sellers");
        }
    }
}
