using System.Text.Json;
using bingo_api.src.Entities;
using bingo_api.src.Structs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RoundMap : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        builder.ToTable("rounds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(r => r.CardValue)
               .HasColumnName("card_value")
               .IsRequired()
               .HasColumnType("numeric(15, 2)");

        builder.Property(r => r.Numbers)
               .HasColumnName("numbers")
               .IsRequired();

        builder.Property(r => r.CardSaleCount)
               .HasColumnName("card_sale_count")
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(r => r.TimeBetweenBalls)
               .HasColumnName("time_between_balls")
               .IsRequired()
               .HasDefaultValue(4);

        builder.Property(r => r.MaxBalls)
               .HasColumnName("max_balls")
               .IsRequired()
               .HasDefaultValue(90);

        builder.Property(r => r.CardRows)
               .HasColumnName("card_rows")
               .IsRequired();

        builder.Property(r => r.CardColumns)
               .HasColumnName("card_columns")
               .IsRequired();

        builder.Property(r => r.Started)
               .HasColumnName("started")
               .IsRequired();

        builder.Property(r => r.Finished)
               .HasColumnName("finished")
               .IsRequired(false)
               .HasDefaultValue(null);

        builder.Property(r => r.RoomId)
               .HasColumnName("room_id")
               .IsRequired();

        builder.Property(r => r.Timeline)
               .HasColumnName("timeline")
               .HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(r => r.Room)
               .WithMany(d => d.Rounds)
               .HasForeignKey(r => r.RoomId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Cards)
               .WithOne(c => c.Round)
               .HasForeignKey(c => c.RoundId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Prizes)
               .WithOne(p => p.Round)
               .HasForeignKey(c => c.RoundId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
