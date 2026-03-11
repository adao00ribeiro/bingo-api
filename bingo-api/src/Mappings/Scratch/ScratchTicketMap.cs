using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ScratchTicketMap : IEntityTypeConfiguration<ScratchTicket>
{
    public void Configure(EntityTypeBuilder<ScratchTicket> builder)
    {
        builder.ToTable("scratch_tickets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Multiplier)
            .HasColumnName("multiplier")
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(x => x.PrizeWon)
            .HasColumnName("prize_won")
            .HasColumnType("numeric(12,2)")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Revealed)
            .HasColumnName("revealed")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.Attributes)
            .HasColumnName("attributes")
            .HasColumnType("jsonb");

        builder.Property(x => x.ScratchSellerGameId)
            .HasColumnName("scratch_seller_game_id")
            .IsRequired();

        builder.Property(x => x.ScratchPrizeId)
            .HasColumnName("scratch_prize_id")
            .IsRequired(false);

        builder.Property(x => x.ScratchBuyId)
            .HasColumnName("scratch_buy_id")
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(x => x.ScratchPrize)
            .WithOne(x => x.ScratchTicket)
            .HasForeignKey<ScratchTicket>(x => x.ScratchPrizeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ScratchBuy)
            .WithMany(x => x.ScratchTickets)
            .HasForeignKey(x => x.ScratchBuyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.ScratchSellerGameId);
        builder.HasIndex(x => x.ScratchBuyId);

        builder.HasIndex(x => x.Attributes)
            .HasMethod("gin");
    }
}