using System;
using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Structs;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class CreateOnlineHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "rooms_sellers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "punters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "payment_methods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "online_houses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    hostname = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    settings = table.Column<OnlineHouseSettings>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DiscardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_houses", x => x.id);
                    table.ForeignKey(
                        name: "FK_online_houses_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rooms_sellers_OnlineHouseId",
                table: "rooms_sellers",
                column: "OnlineHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_OnlineHouseId",
                table: "rooms",
                column: "OnlineHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_punters_OnlineHouseId",
                table: "punters",
                column: "OnlineHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_OnlineHouseId",
                table: "payment_methods",
                column: "OnlineHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_online_houses_hostname",
                table: "online_houses",
                column: "hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_online_houses_seller_id",
                table: "online_houses",
                column: "seller_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_online_houses_OnlineHouseId",
                table: "payment_methods",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_punters_online_houses_OnlineHouseId",
                table: "punters",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_online_houses_OnlineHouseId",
                table: "rooms",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_sellers_online_houses_OnlineHouseId",
                table: "rooms_sellers",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_online_houses_OnlineHouseId",
                table: "payment_methods");

            migrationBuilder.DropForeignKey(
                name: "FK_punters_online_houses_OnlineHouseId",
                table: "punters");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_online_houses_OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_sellers_online_houses_OnlineHouseId",
                table: "rooms_sellers");

            migrationBuilder.DropTable(
                name: "online_houses");

            migrationBuilder.DropIndex(
                name: "IX_rooms_sellers_OnlineHouseId",
                table: "rooms_sellers");

            migrationBuilder.DropIndex(
                name: "IX_rooms_OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_punters_OnlineHouseId",
                table: "punters");

            migrationBuilder.DropIndex(
                name: "IX_payment_methods_OnlineHouseId",
                table: "payment_methods");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "rooms_sellers");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "punters");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "payment_methods");
        }
    }
}
