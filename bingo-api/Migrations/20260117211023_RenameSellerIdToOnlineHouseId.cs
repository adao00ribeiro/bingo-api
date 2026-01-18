using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameSellerIdToOnlineHouseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_online_houses_seller_id",
                table: "payment_methods");

            migrationBuilder.RenameColumn(
                name: "seller_id",
                table: "payment_methods",
                newName: "online_house_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_methods_seller_id",
                table: "payment_methods",
                newName: "IX_payment_methods_online_house_id");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_online_houses_online_house_id",
                table: "payment_methods",
                column: "online_house_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_online_houses_online_house_id",
                table: "payment_methods");

            migrationBuilder.RenameColumn(
                name: "online_house_id",
                table: "payment_methods",
                newName: "seller_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_methods_online_house_id",
                table: "payment_methods",
                newName: "IX_payment_methods_seller_id");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_online_houses_seller_id",
                table: "payment_methods",
                column: "seller_id",
                principalTable: "online_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
