using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueSellerGameConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_scratch_seller_games_seller_id",
                table: "scratch_seller_games");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_seller_id_scratch_game_id",
                table: "scratch_seller_games",
                columns: new[] { "seller_id", "scratch_game_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_scratch_seller_games_seller_id_scratch_game_id",
                table: "scratch_seller_games");

            migrationBuilder.CreateIndex(
                name: "IX_scratch_seller_games_seller_id",
                table: "scratch_seller_games",
                column: "seller_id");
        }
    }
}
