using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Bogus;
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

              builder.Property(r => r.Amount)
                     .HasColumnName("amount")
                     .IsRequired()
                     .HasColumnType("numeric(18, 8)");
                    

              builder.Property(r => r.Status)
                     .HasColumnName("status")
                     .IsRequired()
                     .HasDefaultValue(EPaymentStatus.PENDING);

              builder.Property(r => r.Qrcode)
                     .HasColumnName("qrcode")
                     .IsRequired()
                     .HasMaxLength(200);

              builder.Property(r => r.ImagemQrcode)
                     .HasColumnName("imagem_qrcode")
                     .IsRequired()
                     .HasColumnType("text");  // para textos longos

              builder.Property(r => r.PunterId)
                     .HasColumnName("punter_id")
                     .IsRequired();

              builder.Property(r => r.Network)
                     .HasColumnName("network")
                  .IsRequired(false)
               .HasMaxLength(50);

              builder.Property(r => r.Token)
                     .HasColumnName("token")
                 .IsRequired(false)
               .HasMaxLength(50);

              builder.Property(r => r.DestinationAddress)
                     .HasColumnName("destination_address")
                 .IsRequired(false)
               .HasMaxLength(100);

              builder.Property(r => r.TxHash)
                     .HasColumnName("tx_hash")
                 .IsRequired(false)
               .HasMaxLength(100);

              builder.Property(x => x.CreatedAt)
                     .HasColumnName("created_at")
                     .IsRequired()
                     .HasDefaultValueSql("CURRENT_TIMESTAMP");

              builder.Property(x => x.UpdatedAt)
                     .HasColumnName("updated_at")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP");

              builder.Property(r => r.ConfirmedAt)
                                  .HasColumnName("confirmed_at")
                                  .IsRequired(false);

              builder.Property(r => r.DiscardedAt)
                                  .HasColumnName("discarded_at")
                                  .IsRequired(false);

              builder.HasOne(r => r.Punter)
                     .WithMany(p => p.Recharges)
                     .HasForeignKey(r => r.PunterId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
