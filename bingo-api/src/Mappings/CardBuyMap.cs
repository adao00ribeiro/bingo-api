using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class CardBuyMap : IEntityTypeConfiguration<CardBuy>
{
     public void Configure(EntityTypeBuilder<CardBuy> builder)
    {

        builder.ToTable("CardBuys");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd();
        builder.Property(a => a.Quantity)
              .IsRequired();
        builder.Property(a => a.PunterId)
              .IsRequired();
              builder.Property(a => a.RoundId)
              .IsRequired();
        builder.HasMany(cb => cb.Cards)
        .WithOne(c => c.CardBuy)
        .HasForeignKey(cb => cb.CardBuyId)
          .OnDelete(DeleteBehavior.Cascade);

      
    }
}
