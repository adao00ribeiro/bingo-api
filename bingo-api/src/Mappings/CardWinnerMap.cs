using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class CardWinnerMap : IEntityTypeConfiguration<CardWinner>
{
       public void Configure(EntityTypeBuilder<CardWinner> builder)
       {
              builder.ToTable("CardsWinners");
              builder.HasKey(x => x.Id);
              builder.Property(x => x.Id).ValueGeneratedOnAdd();
              builder.Property(e => e.Value)
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
              builder.Property(e => e.CardId)
                     .IsRequired();

              builder.Property(e => e.PrizeId)
                     .IsRequired();

              builder.HasOne(cw => cw.Card)
                     .WithMany(c => c.CardWinners)
                     .HasForeignKey(e => e.CardId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(cw => cw.Prize)
                     .WithMany(w => w.CardWinners)
                     .HasForeignKey(e => e.PrizeId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
