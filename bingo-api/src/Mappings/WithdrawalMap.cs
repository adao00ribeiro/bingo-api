using bingo_api.src.Entities;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings
{
    public class WithdrawalMap : IEntityTypeConfiguration<Withdrawal>
    {
        public void Configure(EntityTypeBuilder<Withdrawal> builder)
        {
            builder.ToTable("withdrawals");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Amount)
                   .HasColumnName("amount")
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasColumnName("status")
                   .IsRequired()
                   .HasDefaultValue(EPaymentStatus.PENDING);
           
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .IsRequired();

           builder.Property(x => x.ConfirmedAt)
                   .HasColumnName("confirmed_at")
                   .IsRequired(false);

           builder.Property(r => r.DiscardedAt)
                   .HasColumnName("discarded_at")
                   .IsRequired(false);

            builder.Property(x => x.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .IsRequired();
                   
            // 🔥 CONFIGURAÇÃO DO TPH (DISCRIMINATOR)
            builder
                .HasDiscriminator<string>("withdrawal_type")
                .HasValue<PunterWithdrawal>("PunterWithdrawal")
                .HasValue<SellerWithdrawal>("SellerWithdrawal");
        }
    }
}
