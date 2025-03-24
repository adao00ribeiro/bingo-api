using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class addisbotinpunter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBot",
                table: "Punters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBot",
                table: "Punters");
        }
    }
}
