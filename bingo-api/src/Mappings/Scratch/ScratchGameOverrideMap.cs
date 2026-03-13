using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Scratch;

public class ScratchGameOverrideMap : IEntityTypeConfiguration<ScratchGameOverride>
{
    public void Configure(EntityTypeBuilder<ScratchGameOverride> builder)
    {
        builder.ToTable("scratch_game_overrides");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.OnlineHouseId, x.ScratchGameId })
               .IsUnique();

     
        builder.Property(x => x.Title)
               .HasColumnName("title")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(x => x.Subtitle)
               .HasColumnName("subtitle")
               .HasMaxLength(200);

        builder.Property(x => x.CardValue)
               .HasColumnName("card_value")
               .HasColumnType("numeric(15,2)")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
               
       builder.Property(x => x.OnlineHouseId)
               .HasColumnName("online_house_id")
               .IsRequired();

        builder.Property(x => x.ScratchGameId)
               .HasColumnName("scratch_game_id")
               .IsRequired();

        builder.HasOne(x => x.OnlineHouse)
               .WithMany()
               .HasForeignKey(x => x.OnlineHouseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScratchGame)
               .WithMany(x => x.ScratchGameOverrides)
               .HasForeignKey(x => x.ScratchGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ScratchBuys)
               .WithOne(x => x.ScratchGameOverride)
               .HasForeignKey(x => x.ScratchGameOverrideId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}