using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class add_transaction_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrizeBalance",
                table: "Sellers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrizeBalance",
                table: "Punters",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TransactionHistorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistorys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionHistorys");

            migrationBuilder.DropColumn(
                name: "PrizeBalance",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "PrizeBalance",
                table: "Punters");
        }
    }
}
