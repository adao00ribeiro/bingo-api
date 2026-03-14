using System.Text.Json;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Structs.Scratchcard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ScratchTicketMap : IEntityTypeConfiguration<ScratchTicket>
{
    public void Configure(EntityTypeBuilder<ScratchTicket> builder)
    {
        builder.ToTable("scratch_tickets");

        builder.HasKey(x => x.Id);

         builder.Property(x => x.Value)
            .HasColumnName("value")
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(x => x.Attributes)
               .HasColumnName("attributes")
               .HasColumnType("jsonb");

        builder.Property(x => x.ScratchBuyId)
            .HasColumnName("scratch_buy_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasMany(x => x.ScratchPrizes)
            .WithOne(x => x.ScratchTicket)
            .HasForeignKey(x => x.ScratchTicketId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ScratchBuy)
            .WithMany(x => x.ScratchTickets)
            .HasForeignKey(x => x.ScratchBuyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.ScratchBuyId);

        builder.HasIndex(x => x.Attributes)
            .HasMethod("gin");
    }
}