using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineHousesFromSellers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            INSERT INTO online_houses (
                id,
                name,
                hostname,
                seller_id,
                created_at,
                updated_at
            )
            SELECT
                s.id,
                s.email,
                'localhost',
                s.id,
                s.created_at,
                s.updated_at
            FROM sellers s
            WHERE NOT EXISTS (
                SELECT 1
                FROM online_houses oh
                WHERE oh.seller_id = s.id
            );
        """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            DELETE FROM online_houses oh
            WHERE EXISTS (
                SELECT 1
                FROM sellers s
                WHERE s.id = oh."SellerId"
            );
        """);
        }
    }
}
