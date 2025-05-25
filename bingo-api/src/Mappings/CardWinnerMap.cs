using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class CardWinnerMap : IEntityTypeConfiguration<CardWinner>
{
    public void Configure(EntityTypeBuilder<CardWinner> builder)
    {
        builder.ToTable("cards_winners");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Value)
               .HasColumnName("value")
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.CardId)
               .HasColumnName("card_id")
               .IsRequired();

        builder.Property(e => e.PrizeId)
               .HasColumnName("prize_id")
               .IsRequired();
        builder.Property(x => x.CreateAt)
                    .HasColumnName("create_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("update_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(cw => cw.Card)
               .WithMany(c => c.CardWinners)
               .HasForeignKey(e => e.CardId)
               .HasConstraintName("fk_card_winner_card_id")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cw => cw.Prize)
               .WithMany(w => w.CardWinners)
               .HasForeignKey(e => e.PrizeId)
               .HasConstraintName("fk_card_winner_prize_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
