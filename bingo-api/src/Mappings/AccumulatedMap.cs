using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class AccumulatedMap : IEntityTypeConfiguration<Accumulated>
{
    public void Configure(EntityTypeBuilder<Accumulated> builder)
    {

        builder.ToTable("Accumulateds");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()

        ;
        builder.Property(a => a.Activated)
              .IsRequired();

        builder.Property(a => a.MinimumValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.MaximumValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.CurrentValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.MaximumNumberOfBalls)
               .IsRequired();

        builder.Property(a => a.CumulativePercentage)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(a => a.IncrementBallCumulative)
               .IsRequired();

        // Relacionamento com Room (um-para-um)
        builder.HasOne(a => a.Room)
               .WithOne(r => r.Accumulated)
               .HasForeignKey<Accumulated>(a => a.RoomId)
               .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
    }
}
