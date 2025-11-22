using System.Text.Json;
using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class ScratchBuyMap : IEntityTypeConfiguration<ScratchBuy>
{
    public void Configure(EntityTypeBuilder<ScratchBuy> builder)
    {

        builder.ToTable("scratch_buys");

        // Chave primária
        builder.HasKey(x => x.Id);

        // Quantidade de bilhetes comprados
        builder.Property(x => x.Quantity)
               .HasColumnName("quantity")
               .IsRequired()
               .HasColumnType("integer");

        // ID do jogo do vendedor
        builder.Property(x => x.SellerGameId)
               .HasColumnName("seller_game_id")
               .IsRequired();

        // ID do apostador
        builder.Property(x => x.PunterId)
               .HasColumnName("punter_id")
               .IsRequired();

        // Timestamps
        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relacionamentos
        builder.HasMany(x => x.ScratchTickets)
               .WithOne(t => t.ScratchBuy)
               .HasForeignKey(t => t.ScratchBuyId)
               .OnDelete(DeleteBehavior.SetNull); // Permite manter tickets mesmo se a compra for excluída
    }
}
