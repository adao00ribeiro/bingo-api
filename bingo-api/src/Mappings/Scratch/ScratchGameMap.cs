using System.Text.Json;
using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace bingo_api.src.Mappings;

public class ScratchGameMap : IEntityTypeConfiguration<ScratchGame>
{
    public void Configure(EntityTypeBuilder<ScratchGame> builder)
    {
        builder.ToTable("scratch_games");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.QuantityToAward)
            .HasColumnName("quantity_to_award")
            .IsRequired();

        builder.Property(x => x.Rows)
            .HasColumnName("rows")
            .IsRequired();

        builder.Property(x => x.Cols)
            .HasColumnName("cols")
            .IsRequired();

        builder.Property(x => x.Component)
            .HasColumnName("component")
            .HasMaxLength(100);

        builder.Property(x => x.Rtp)
            .HasColumnName("rtp")
            .HasColumnType("numeric(10,4)");

        builder.Property(x => x.AllowedMultipliers)
            .HasColumnName("allowed_multipliers")
            .HasColumnType("integer[]");

        builder.Property(x => x.PayoutTable)
            .HasColumnName("payout_table")
            .HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasMany(x => x.ScratchSellerGames)
            .WithOne(x => x.ScratchGame)
            .HasForeignKey(x => x.ScratchGameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}