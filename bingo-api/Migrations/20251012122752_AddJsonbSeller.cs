using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Structs;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonbSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<SellerSettings>(
                name: "settings",
                table: "sellers",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "settings",
                table: "sellers");
        }
    }
}
