using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class ScratchBuyMap : IEntityTypeConfiguration<ScratchBuy>
{
    public void Configure(EntityTypeBuilder<ScratchBuy> builder)
    {
        builder.ToTable("scratch_buys");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
               .HasColumnName("value")
               .HasColumnType("numeric(15,2)")
               .IsRequired();

        builder.Property(x => x.Quantity)
               .HasColumnName("quantity")
               .IsRequired();

        builder.Property(x => x.ScratchGameOverrideId)
               .HasColumnName("scratch_game_override_id")
               .IsRequired();

        builder.Property(x => x.PunterId)
               .HasColumnName("punter_id")
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .IsRequired();

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .IsRequired();

        builder.HasOne<ScratchGameOverride>()
               .WithMany(x => x.ScratchBuys)
               .HasForeignKey(x => x.ScratchGameOverrideId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Punter)
               .WithMany()
               .HasForeignKey(x => x.PunterId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ScratchTickets)
               .WithOne(x => x.ScratchBuy)
               .HasForeignKey(x => x.ScratchBuyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ScratchGameOverrideId);
        builder.HasIndex(x => x.PunterId);
    }
}