using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bingo_api.src.Mappings;

public class RoundMap : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.ToTable("Rounds");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        // Propriedades
        builder.Property(r => r.CardValue)
            .IsRequired().HasColumnType("numeric(15, 2)");

        builder.Property(r => r.Numbers)
            .IsRequired();

        builder.Property(r => r.CardSaleCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(r => r.TimeBetweenBalls)
            .IsRequired()
            .HasDefaultValue(4);

        builder.Property(r => r.MaxBalls)
          .IsRequired()
          .HasDefaultValue(90);

        builder.Property(r => r.CardRows)
      .IsRequired();
        builder.Property(r => r.CardColumns)
      .IsRequired();

        builder.Property(r => r.Started)
            .IsRequired();

        builder.Property(r => r.Finished)
         .IsRequired(false) // Não obrigatório
    .HasDefaultValue(null); // Garante que o valor padrão será NULL


        builder.HasOne(r => r.Room)
            .WithMany(d => d.Rounds)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasMany(r => r.Cards)
            .WithOne(c => c.Round)
            .HasForeignKey(c => c.RoundId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Prizes)
                .WithOne(p => p.Round) // Se não houver uma propriedade de navegação na classe Prize que se refira a Round, pode deixar vazio
                  .HasForeignKey(c => c.RoundId)
                .OnDelete(DeleteBehavior.Cascade);


    }
}
