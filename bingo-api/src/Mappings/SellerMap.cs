using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class SellerMap : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("sellers");

        builder.HasKey(x => x.Id); // Definindo a chave primária
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(x => x.CreateAt)
               .HasColumnName("create_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("update_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Balance)
               .HasColumnName("balance")
               .IsRequired()
               .HasColumnType("numeric(15, 2)");


        builder.Property(x => x.IndicateRewardValue)
               .HasColumnName("indicate_reward_value")
               .HasDefaultValue(20.0)
               .HasColumnType("numeric(15, 2)");

        builder.Property(x => x.Comission)
               .HasColumnName("comission")
               .IsRequired()
         .HasColumnType("numeric(15, 2)");

        builder.Property(x => x.Email)
               .HasColumnName("email")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Cpf)
               .HasColumnName("cpf")
               .HasMaxLength(11)
               .IsRequired();

        builder.Property(x => x.DateBirth)
               .HasColumnName("date_birth")
               .IsRequired();

        builder.HasMany(s => s.OwnerRooms)
               .WithOne(r => r.Owner)
               .HasForeignKey(r => r.OwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Rooms)
               .WithOne(rs => rs.Seller)
               .HasForeignKey(rs => rs.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Punters)
               .WithOne(p => p.Seller)
               .HasForeignKey(p => p.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.PaymentMethods)
               .WithOne(pm => pm.Seller)
               .HasForeignKey(pm => pm.SellerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
