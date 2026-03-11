using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class ScratchPrizeMap : IEntityTypeConfiguration<ScratchPrize>
{
    public void Configure(EntityTypeBuilder<ScratchPrize> builder)
    {
        builder.ToTable("scratch_prizes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(x => x.ScratchTicketId)
            .HasColumnName("scratch_ticket_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(x => x.ScratchTicket)
            .WithOne(x => x.ScratchPrize)
            .HasForeignKey<ScratchPrize>(x => x.ScratchTicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ScratchTicketId)
            .IsUnique();
    }
}