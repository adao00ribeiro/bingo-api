using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bingo_api.src.Mappings;

public class SellerMap : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("Sellers");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.CreateAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdateAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.Balance).IsRequired().HasColumnType("numeric(15, 2)");
        builder.Property(x => x.Comission).IsRequired().HasColumnType("numeric(15, 2)");
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Cpf).HasMaxLength(11).IsRequired();
        builder.Property(x => x.DateBirth).IsRequired();

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

    }
}