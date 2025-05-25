using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RechargeMap : IEntityTypeConfiguration<Recharge>
{
    public void Configure(EntityTypeBuilder<Recharge> builder)
    {
        builder.ToTable("recharges");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(r => r.Value)
               .HasColumnName("value")
               .IsRequired()
               .HasColumnType("numeric(15, 2)");

        builder.Property(r => r.Status)
               .HasColumnName("status")
               .IsRequired()
               .HasDefaultValue(ERechargeStatus.PENDING);

        builder.Property(r => r.Qrcode)
               .HasColumnName("qrcode")
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(r => r.ImagemQrcode)
               .HasColumnName("imagem_qrcode")
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(r => r.PunterId)
               .HasColumnName("punter_id");
        builder.Property(x => x.CreateAt)
                    .HasColumnName("create_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("update_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(r => r.Punter)
               .WithMany(p => p.Recharges)
               .HasForeignKey(r => r.PunterId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
