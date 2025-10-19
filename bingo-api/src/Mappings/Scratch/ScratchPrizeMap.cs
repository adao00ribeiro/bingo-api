using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class ScratchPrizeMap : IEntityTypeConfiguration<ScratchPrize>
{
    public void Configure(EntityTypeBuilder<ScratchPrize> builder)
    {
        builder.ToTable("scratch_prizes");

        // Chave primária
        builder.HasKey(x => x.Id);

        // Descrição do prêmio
        builder.Property(x => x.Description)
               .HasColumnName("description")
               .IsRequired()
               .HasMaxLength(200);

        // Valor do prêmio
        builder.Property(x => x.Amount)
               .HasColumnName("amount")
               .IsRequired()
               .HasColumnType("numeric(12,2)");

        // ID do jogo
        builder.Property(x => x.ScratchGameId)
               .HasColumnName("scratch_game_id")
               .IsRequired();

        // ID do bilhete
        builder.Property(x => x.ScratchTicketId)
               .HasColumnName("scratch_ticket_id")
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
        builder.HasOne(x => x.ScratchGame)
               .WithMany(g => g.ScratchPrizes)
               .HasForeignKey(x => x.ScratchGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScratchTicket)
               .WithOne(t => t.ScratchPrize)
               .HasForeignKey<ScratchPrize>(x => x.ScratchTicketId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}