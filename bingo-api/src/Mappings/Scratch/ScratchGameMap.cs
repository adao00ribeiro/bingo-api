using System.Text.Json;
using bingo_api.src.Entities.Scratch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class ScratchGameMap : IEntityTypeConfiguration<ScratchGame>
{
    public void Configure(EntityTypeBuilder<ScratchGame> builder)
    {
      
        builder.ToTable("scratch_games");

        // Chave primária
        builder.HasKey(x => x.Id);

             // Nome do jogo
        builder.Property(x => x.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(100);

        // Layout (ex: 3x3, 1x15)
        builder.Property(x => x.LayoutType)
               .HasColumnName("layout_type")
               .IsRequired()
               .HasMaxLength(10);


        // Preço por bilhete
        builder.Property(x => x.Price)
               .HasColumnName("price")
               .IsRequired()
               .HasColumnType("numeric(10,2)");

        // Prêmio máximo
        builder.Property(x => x.MaxPrize)
               .HasColumnName("max_prize")
               .IsRequired()
               .HasColumnType("numeric(12,2)");

        // Probabilidade (ex: 3.1)
        builder.Property(x => x.Probability)
               .HasColumnName("probability")
               .HasColumnType("numeric(5,2)")
               .IsRequired(false);


        // Multiplicadores permitidos (ex: [1, 5, 10, 25])
        builder.Property(x => x.AllowedMultipliers)
               .HasColumnName("allowed_multipliers")
               .HasColumnType("integer[]");

              // Tabela de pagamento (ex: { "1x": 5.00, "10x": 50.00 })
       builder.Property(x => x.Attributes)
                     .HasColumnName("attributes")
                     .HasColumnType("jsonb");
        // Timestamps
        builder.Property(x => x.CreateAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("updated_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
  // Relacionamentos
        builder.HasMany(x => x.ScratchSellerGames)
               .WithOne(s => s.ScratchGame)
               .HasForeignKey(s => s.ScratchGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ScratchPrizes)
               .WithOne(p => p.ScratchGame)
               .HasForeignKey(p => p.ScratchGameId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
