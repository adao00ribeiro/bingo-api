using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bingo_api.Migrations
{
    public partial class UpdateWithdrawal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // renomear coluna
            migrationBuilder.RenameColumn(
                name: "DiscardedAt",
                table: "withdrawals",
                newName: "discarded_at");

            // 1️⃣ Garantir que valores string virem int
            migrationBuilder.Sql(@"
                UPDATE withdrawals SET status =
                    CASE status
                        WHEN 'PENDING' THEN 0
                        WHEN 'APPROVED' THEN 1
                        WHEN 'REJECTED' THEN 2
                        ELSE 0
                    END;
            ");

            // 2️⃣ Alterar coluna para INTEGER usando CAST
            migrationBuilder.Sql(@"
                ALTER TABLE withdrawals 
                ALTER COLUMN status 
                TYPE integer 
                USING status::integer;
            ");

            // 3️⃣ Aplicar default e not null corretamente
            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "withdrawals",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text");

            // adicionar nova coluna
            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                table: "withdrawals",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remover coluna adicionada
            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "withdrawals");

            // 1️⃣ Converter int → texto novamente
            migrationBuilder.Sql(@"
                UPDATE withdrawals SET status =
                    CASE status
                        WHEN 0 THEN 'PENDING'
                        WHEN 1 THEN 'APPROVED'
                        WHEN 2 THEN 'REJECTED'
                        ELSE 'PENDING'
                    END;
            ");

            // 2️⃣ Alterar coluna para texto novamente
            migrationBuilder.Sql(@"
                ALTER TABLE withdrawals 
                ALTER COLUMN status 
                TYPE text 
                USING status::text;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "withdrawals",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            // desfazer rename
            migrationBuilder.RenameColumn(
                name: "discarded_at",
                table: "withdrawals",
                newName: "DiscardedAt");
        }
    }
}
