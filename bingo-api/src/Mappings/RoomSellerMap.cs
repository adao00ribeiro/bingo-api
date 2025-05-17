using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RoomSellerMap : IEntityTypeConfiguration<RoomSeller>
{
    public void Configure(EntityTypeBuilder<RoomSeller> builder)
    {
        builder.ToTable("rooms_sellers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(rs => rs.AssignedBy)
               .HasColumnName("assigned_by")
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(rs => rs.RoomId)
               .HasColumnName("room_id");

        builder.Property(rs => rs.SellerId)
               .HasColumnName("seller_id");
   builder.Property(x => x.CreateAt)
               .HasColumnName("create_at")
               .IsRequired()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdateAt)
               .HasColumnName("update_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(rs => rs.Room)
               .WithMany(r => r.RoomsSellers)
               .HasForeignKey(rs => rs.RoomId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rs => rs.Seller)
               .WithMany(s => s.Rooms)
               .HasForeignKey(rs => rs.SellerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
