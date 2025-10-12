using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class CardMap : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
               .HasColumnName("name")
               .IsRequired();

        builder.Property(e => e.Numbers)
               .HasColumnName("numbers")
               .IsRequired();

        builder.Property(e => e.Code)
               .HasColumnName("code")
               .IsRequired();

        builder.Property(e => e.RoundId)
               .HasColumnName("round_id")
               .IsRequired();

        builder.Property(e => e.PunterId)
               .HasColumnName("punter_id")
               .IsRequired();

        builder.Property(e => e.CardBuyId)
               .HasColumnName("card_buy_id")
               .IsRequired();
        builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(c => c.Round)
               .WithMany(r => r.Cards)
               .HasForeignKey(e => e.RoundId)
               .HasConstraintName("fk_card_round_id")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Punter)
               .WithMany(p => p.Cards)
               .HasForeignKey(e => e.PunterId)
               .HasConstraintName("fk_card_punter_id")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.CardBuy)
               .WithMany(c => c.Cards)
               .HasForeignKey(cb => cb.CardBuyId)
               .HasConstraintName("fk_card_card_buy_id")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.CardWinners)
               .WithOne(cw => cw.Card)
               .HasForeignKey(cw => cw.CardId)
               .HasConstraintName("fk_card_winner_card_id")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
