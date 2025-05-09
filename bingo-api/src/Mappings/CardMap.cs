using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bingo_api.src.Mappings;

public class CardMap : IEntityTypeConfiguration<Card>
{
       public void Configure(EntityTypeBuilder<Card> builder)
       {

              builder.ToTable("Cards");
              builder.HasKey(x => x.Id);
              builder.Property(x => x.Id)
              .ValueGeneratedOnAdd();

              builder.Property(e => e.Name)
                    .IsRequired();

              builder.Property(e => e.Numbers)
                  .IsRequired();

              builder.Property(e => e.Code)
                   .IsRequired();

              builder.Property(e => e.RoundId)
                     .IsRequired();

              builder.Property(e => e.PunterId)
                     .IsRequired();

              builder.Property(e => e.CardBuyId)
                     .IsRequired();

              builder.HasOne(c => c.Round)
                     .WithMany(r => r.Cards)
                     .HasForeignKey(e => e.RoundId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(c => c.Punter)
                     .WithMany(p => p.Cards)
                     .HasForeignKey(e => e.PunterId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(c => c.CardWinners)
                     .WithOne(cw => cw.Card)
                     .HasForeignKey(cw => cw.CardId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasOne(cb => cb.CardBuy)
                     .WithMany(c => c.Cards)
                     .HasForeignKey(cb => cb.CardBuyId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
