using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class PrizeMap : IEntityTypeConfiguration<Prize>
{
    public void Configure(EntityTypeBuilder<Prize> builder)
    {
        builder.ToTable("prizes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(p => p.Value)
               .HasColumnName("value")
               .IsRequired()
               .HasColumnType("numeric(15, 2)");

        builder.Property(p => p.Type)
               .HasColumnName("type")
               .IsRequired();

        builder.Property(p => p.RoundId)
               .HasColumnName("round_id")
               .IsRequired();
        builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(p => p.Round)
               .WithMany(d => d.Prizes)
               .HasForeignKey(p => p.RoundId)
               .HasConstraintName("fk_prize_round_id")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.CardWinners)
               .WithOne(cw => cw.Prize)
               .HasForeignKey(cw => cw.PrizeId)
               .HasConstraintName("fk_card_winner_prize_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
