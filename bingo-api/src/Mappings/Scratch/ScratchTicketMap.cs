using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ScratchTicketMap : IEntityTypeConfiguration<ScratchTicket>
{
    public void Configure(EntityTypeBuilder<ScratchTicket> builder)
    {

        builder.ToTable("scratch_tickets");

        // Chave primária
        builder.HasKey(x => x.Id);

        // Multiplicador
        builder.Property(x => x.Multiplier)
               .HasColumnName("multiplier")
               .IsRequired()
               .HasDefaultValue(1);

        // Prêmio ganho
        builder.Property(x => x.PrizeWon)
               .HasColumnName("prize_won")
               .IsRequired()
               .HasColumnType("numeric(12,2)")
               .HasDefaultValue(0);

        // Revelado
        builder.Property(x => x.Revealed)
               .HasColumnName("revealed")
               .IsRequired()
               .HasDefaultValue(false);

        // Atributos (PunterId e Items)
        builder.Property(x => x.Attributes)
               .HasColumnName("attributes")
               .HasColumnType("jsonb");

        // ID do jogo do vendedor
        builder.Property(x => x.ScratchSellerGameId)
               .HasColumnName("scratch_seller_game_id")
               .IsRequired();

        // ID do prêmio (opcional)
        builder.Property(x => x.ScratchPrizeId)
               .HasColumnName("scratch_prize_id")
               .IsRequired(false);

        // ID da compra (opcional)
        builder.Property(x => x.ScratchBuyId)
               .HasColumnName("scratch_buy_id")
               .IsRequired(false);

        // Timestamps
        builder.Property(x => x.CreateAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("updated_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relacionamentos
        builder.HasOne(x => x.ScratchSellerGame)
               .WithMany(s => s.ScratchTickets)
               .HasForeignKey(x => x.ScratchSellerGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScratchPrize)
               .WithOne(p => p.ScratchTicket)
               .HasForeignKey<ScratchTicket>(x => x.ScratchPrizeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ScratchBuy)
               .WithMany(b => b.ScratchTickets)
               .HasForeignKey(x => x.ScratchBuyId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
