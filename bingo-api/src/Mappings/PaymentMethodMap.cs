using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class PaymentMethodMap : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(pm => pm.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(pm => pm.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(pm => pm.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(pm => pm.Type)
               .HasColumnName("type")
               .IsRequired();

        builder.Property(pm => pm.Token)
               .HasColumnName("token")
               .HasMaxLength(255)
               .IsRequired(false);

        builder.Property(pm => pm.QrCodeUrl)
               .HasColumnName("qrcode_url")
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(pm => pm.Instructions)
               .HasColumnName("instructions")
               .HasColumnType("text")
               .IsRequired(false);

        builder.Property(pm => pm.Active)
               .HasColumnName("active")
               .HasDefaultValue(true)
               .IsRequired();

        builder.Property(pm => pm.SellerId)
               .HasColumnName("seller_id")
               .IsRequired();

        builder.HasOne(pm => pm.Seller)
               .WithMany(s => s.PaymentMethods)
               .HasForeignKey(pm => pm.SellerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}