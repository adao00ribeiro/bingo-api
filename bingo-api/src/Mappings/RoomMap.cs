using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RoomMap : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        // Propriedades
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(r => r.Owner)
        .WithMany(s => s.OwnerRooms)
        .HasForeignKey(r => r.OwnerId)
        .OnDelete(DeleteBehavior.Restrict); // Evita cascata de deleção ao deletar um Seller

        builder.HasMany(r => r.RoomsSellers)
        .WithOne(rs => rs.Room)
        .HasForeignKey(rs => rs.RoomId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Rounds)
        .WithOne(d => d.Room)
        .HasForeignKey(d => d.RoomId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Accumulated)
        .WithOne(a => a.Room)
        .HasForeignKey<Accumulated>(a => a.RoomId)
        .IsRequired();

         builder.HasOne(r => r.BotConfig)
        .WithOne(a => a.Room)
        .HasForeignKey<BotConfig>(a => a.RoomId)
        .IsRequired();
    }
}
