using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using bingo_api.src.Structs;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class timeline_in_round : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<TimelineEvent>>(
                name: "Timeline",
                table: "Rounds",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");  
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timeline",
                table: "Rounds");
        }
    }
}
