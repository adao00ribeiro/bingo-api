using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bingo_api.src.Mappings;

public class PrizeMap : IEntityTypeConfiguration<Prize>
{
    public void Configure(EntityTypeBuilder<Prize> builder)
    {
        builder.ToTable("Prizes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Value)
            .IsRequired().HasColumnType("numeric(15, 2)"); // Ajuste a precisão e a escala conforme necessário
        // Mapeamento de propriedades específicas
        builder.Property(p => p.Type)
            .IsRequired();
        // Relacionamento com round
        builder.HasOne(p => p.Round)
            .WithMany(d => d.Prizes)
            .HasForeignKey(p => p.RoundId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com Winner (um-para-muitos)
        builder.HasMany(p => p.CardWinners)
            .WithOne(cw => cw.Prize)
            .HasForeignKey(cw => cw.PrizeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
