using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bingo_api.src.Mappings;

public class RechargeMap : IEntityTypeConfiguration<Recharge>
{
    public void Configure(EntityTypeBuilder<Recharge> builder)
    {
        builder.ToTable("Recharges");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        // Propriedades
        builder.Property(r => r.Value)
            .IsRequired().HasColumnType("numeric(15, 2)");

        builder.Property(r => r.Status)
            .IsRequired()
            .HasDefaultValue(ERechargeStatus.PENDING);

        builder.Property(r => r.Qrcode)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.ImagemQrcode)
            .IsRequired()
            .HasMaxLength(500);

        // Relacionamento com Punter (muitos-para-um)
        builder.HasOne(r => r.Punter)
            .WithMany(p => p.Recharges)
            .HasForeignKey(r => r.PunterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
