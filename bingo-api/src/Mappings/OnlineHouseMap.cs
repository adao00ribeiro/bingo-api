using bingo_api.src.Entities;
using bingo_api.src.Entities.Bingo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings.Bingo;

public class OnlineHouseMap : IEntityTypeConfiguration<OnlineHouse>
{
    public void Configure(EntityTypeBuilder<OnlineHouse> builder)
    {
        builder.ToTable("online_houses");
        // PK
        builder.HasKey(x => x.Id);


        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();
        // Columns
        builder.Property(x => x.Name)
          .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Hostname)
            .HasColumnName("hostname")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.SellerId)
          .HasColumnName("seller_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
        // 1:1 Seller <-> OnlineHouse
        builder.HasOne(x => x.Seller)
            .WithOne(s => s.OnlineHouse)
            .HasForeignKey<OnlineHouse>(x => x.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
        // Media Attachments (opcional)

        builder.Property(s => s.Settings)
            .HasColumnName("settings")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'{}'::jsonb");

 
   builder.HasMany(x => x.Punters)
       .WithOne(p => p.OnlineHouse)
       .HasForeignKey(p => p.OnlineHouseId)
       .OnDelete(DeleteBehavior.Restrict);

   builder.HasMany(x => x.OwnerRooms)
       .WithOne(r => r.Owner)
       .HasForeignKey(r => r.OwnerId)
       .OnDelete(DeleteBehavior.Cascade);
  
   builder.HasMany(x => x.PaymentMethods)
       .WithOne(pm => pm.OnlineHouse)
       .HasForeignKey(pm => pm.OnlineHouseId)
       .OnDelete(DeleteBehavior.Cascade);
  

        //      builder.Ignore(x => x.NavBarMediaAttachment);
        //      builder.Ignore(x => x.LoginLogoMediaAttachment);
        // Indexes
        builder.HasIndex(x => x.Hostname).IsUnique();
        builder.HasIndex(x => x.SellerId).IsUnique();
    }
}