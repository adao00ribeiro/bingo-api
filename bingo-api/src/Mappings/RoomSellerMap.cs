using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RoomSellerMap : IEntityTypeConfiguration<RoomSeller>
{
    public void Configure(EntityTypeBuilder<RoomSeller> builder)
    {
        builder.ToTable("RoomsSellers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        // Propriedades
        builder.Property(rs => rs.AssignedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(rs => rs.Room)
                       .WithMany(r => r.RoomsSellers)
                       .HasForeignKey(rs => rs.RoomId)
                       .OnDelete(DeleteBehavior.Cascade);

        // Configurando relacionamento com Seller
        builder.HasOne(rs => rs.Seller)
               .WithMany(s => s.Rooms)
               .HasForeignKey(rs => rs.SellerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
