using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    DateBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comission = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Punters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    DateBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Punters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Punters_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Sellers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Qrcode = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    ImagemQrcode = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    PunterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recharges_Punters_PunterId",
                        column: x => x.PunterId,
                        principalTable: "Punters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accumulateds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Activated = table.Column<bool>(type: "boolean", nullable: false),
                    MinimumValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaximumValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaximumNumberOfBalls = table.Column<int>(type: "integer", nullable: false),
                    CumulativePercentage = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IncrementBallCumulative = table.Column<bool>(type: "boolean", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accumulateds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accumulateds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsSellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedBy = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsSellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsSellers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsSellers_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardValue = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Numbers = table.Column<int[]>(type: "integer[]", nullable: false),
                    CardSaleCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TimeBetweenBalls = table.Column<int>(type: "integer", nullable: false, defaultValue: 4),
                    MaxBalls = table.Column<int>(type: "integer", nullable: false, defaultValue: 90),
                    CardRows = table.Column<int>(type: "integer", nullable: false),
                    CardColumns = table.Column<int>(type: "integer", nullable: false),
                    Started = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Finished = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Numbers = table.Column<int[]>(type: "integer[]", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    RoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    PunterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Punters_PunterId",
                        column: x => x.PunterId,
                        principalTable: "Punters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prizes_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardsWinners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrizeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsWinners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardsWinners_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardsWinners_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accumulateds_RoomId",
                table: "Accumulateds",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PunterId",
                table: "Cards",
                column: "PunterId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RoundId",
                table: "Cards",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsWinners_CardId",
                table: "CardsWinners",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsWinners_PrizeId",
                table: "CardsWinners",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_RoundId",
                table: "Prizes",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Punters_SellerId",
                table: "Punters",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recharges_PunterId",
                table: "Recharges",
                column: "PunterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_OwnerId",
                table: "Rooms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSellers_RoomId",
                table: "RoomsSellers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSellers_SellerId",
                table: "RoomsSellers",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomId",
                table: "Rounds",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accumulateds");

            migrationBuilder.DropTable(
                name: "CardsWinners");

            migrationBuilder.DropTable(
                name: "Recharges");

            migrationBuilder.DropTable(
                name: "RoomsSellers");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Prizes");

            migrationBuilder.DropTable(
                name: "Punters");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Sellers");
        }
    }
}
