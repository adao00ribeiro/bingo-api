using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class CardBuyMap : IEntityTypeConfiguration<CardBuy>
{
    public void Configure(EntityTypeBuilder<CardBuy> builder)
    {
        builder.ToTable("card_buys");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Quantity)
               .HasColumnName("quantity")
               .IsRequired();

        builder.Property(a => a.PunterId)
               .HasColumnName("punter_id")
               .IsRequired();

        builder.Property(a => a.RoundId)
               .HasColumnName("round_id")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
             .HasColumnName("created_at")
             .IsRequired()
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasMany(cb => cb.Cards)
               .WithOne(c => c.CardBuy)
               .HasForeignKey(cb => cb.CardBuyId)
               .HasConstraintName("fk_card_cardbuy_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
