using System;
using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Structs;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AjustReferenceOnlineHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_online_houses_OnlineHouseId",
                table: "payment_methods");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_sellers_seller_id",
                table: "payment_methods");

            migrationBuilder.DropForeignKey(
                name: "FK_punters_online_houses_OnlineHouseId",
                table: "punters");

            migrationBuilder.DropForeignKey(
                name: "FK_punters_sellers_seller_id",
                table: "punters");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_online_houses_OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_sellers_owner_id",
                table: "rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_sellers_online_houses_OnlineHouseId",
                table: "rooms_sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_sellers_sellers_seller_id",
                table: "rooms_sellers");

            migrationBuilder.DropIndex(
                name: "IX_rooms_sellers_seller_id",
                table: "rooms_sellers");

            migrationBuilder.DropIndex(
                name: "IX_rooms_OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_punters_seller_id",
                table: "punters");

            migrationBuilder.DropIndex(
                name: "IX_payment_methods_OnlineHouseId",
                table: "payment_methods");

            migrationBuilder.DropColumn(
                name: "settings",
                table: "sellers");

            migrationBuilder.DropColumn(
                name: "seller_id",
                table: "rooms_sellers");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "seller_id",
                table: "punters");

            migrationBuilder.DropColumn(
                name: "OnlineHouseId",
                table: "payment_methods");

            migrationBuilder.RenameColumn(
                name: "OnlineHouseId",
                table: "rooms_sellers",
                newName: "online_house_id");

            migrationBuilder.RenameIndex(
                name: "IX_rooms_sellers_OnlineHouseId",
                table: "rooms_sellers",
                newName: "IX_rooms_sellers_online_house_id");

            migrationBuilder.RenameColumn(
                name: "OnlineHouseId",
                table: "punters",
                newName: "online_house_id");

            migrationBuilder.RenameIndex(
                name: "IX_punters_OnlineHouseId",
                table: "punters",
                newName: "IX_punters_online_house_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "online_house_id",
                table: "rooms_sellers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "online_house_id",
                table: "punters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_online_houses_seller_id",
                table: "payment_methods",
                column: "seller_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_punters_online_houses_online_house_id",
                table: "punters",
                column: "online_house_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_online_houses_owner_id",
                table: "rooms",
                column: "owner_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_sellers_online_houses_online_house_id",
                table: "rooms_sellers",
                column: "online_house_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_online_houses_seller_id",
                table: "payment_methods");

            migrationBuilder.DropForeignKey(
                name: "FK_punters_online_houses_online_house_id",
                table: "punters");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_online_houses_owner_id",
                table: "rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_sellers_online_houses_online_house_id",
                table: "rooms_sellers");

            migrationBuilder.RenameColumn(
                name: "online_house_id",
                table: "rooms_sellers",
                newName: "OnlineHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_rooms_sellers_online_house_id",
                table: "rooms_sellers",
                newName: "IX_rooms_sellers_OnlineHouseId");

            migrationBuilder.RenameColumn(
                name: "online_house_id",
                table: "punters",
                newName: "OnlineHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_punters_online_house_id",
                table: "punters",
                newName: "IX_punters_OnlineHouseId");

            migrationBuilder.AddColumn<SellerSettings>(
                name: "settings",
                table: "sellers",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");

            migrationBuilder.AlterColumn<Guid>(
                name: "OnlineHouseId",
                table: "rooms_sellers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "seller_id",
                table: "rooms_sellers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OnlineHouseId",
                table: "punters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "seller_id",
                table: "punters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OnlineHouseId",
                table: "payment_methods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_sellers_seller_id",
                table: "rooms_sellers",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_OnlineHouseId",
                table: "rooms",
                column: "OnlineHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_punters_seller_id",
                table: "punters",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_OnlineHouseId",
                table: "payment_methods",
                column: "OnlineHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_online_houses_OnlineHouseId",
                table: "payment_methods",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_sellers_seller_id",
                table: "payment_methods",
                column: "seller_id",
                principalTable: "sellers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_punters_online_houses_OnlineHouseId",
                table: "punters",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_punters_sellers_seller_id",
                table: "punters",
                column: "seller_id",
                principalTable: "sellers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_online_houses_OnlineHouseId",
                table: "rooms",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_sellers_owner_id",
                table: "rooms",
                column: "owner_id",
                principalTable: "sellers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_sellers_online_houses_OnlineHouseId",
                table: "rooms_sellers",
                column: "OnlineHouseId",
                principalTable: "online_houses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_sellers_sellers_seller_id",
                table: "rooms_sellers",
                column: "seller_id",
                principalTable: "sellers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
