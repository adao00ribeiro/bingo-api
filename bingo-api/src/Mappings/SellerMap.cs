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

        builder.Property(x => x.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
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

    }
}
