using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class CryptRecharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "recharges");

            migrationBuilder.DropColumn(
                name: "destination_address",
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
        }
    }
}
