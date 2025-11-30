using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Scratch;

public class ScratchSellerGameMap : IEntityTypeConfiguration<ScratchSellerGame>
{
    public void Configure(EntityTypeBuilder<ScratchSellerGame> builder)
    {
        builder.ToTable("scratch_seller_games");

        // Chave primária
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.SellerId, x.ScratchGameId })
               .IsUnique();
        // ID do vendedor
        builder.Property(x => x.SellerId)
               .HasColumnName("seller_id")
               .IsRequired();

        // ID do jogo
        builder.Property(x => x.ScratchGameId)
               .HasColumnName("scratch_game_id")
               .IsRequired();

        // Timestamps
        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Relacionamentos
        builder.HasOne(x => x.Seller)
               .WithMany()
               .HasForeignKey(x => x.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScratchGame)
               .WithMany(g => g.ScratchSellerGames)
               .HasForeignKey(x => x.ScratchGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ScratchTickets)
               .WithOne(t => t.ScratchSellerGame)
               .HasForeignKey(t => t.ScratchSellerGameId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}