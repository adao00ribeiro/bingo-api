using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    /// <inheritdoc />
    public partial class MediaAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "media_attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiscardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_media_attachments_rooms_EntityId",
                        column: x => x.EntityId,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_media_attachments_EntityId",
                table: "media_attachments",
                column: "EntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_media_attachments_EntityType_EntityId",
                table: "media_attachments",
                columns: new[] { "EntityType", "EntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_attachments");
        }
    }
}
