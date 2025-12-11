using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class RoomMap : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(r => r.OwnerId)
               .HasColumnName("owner_id");
        builder.Property(x => x.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

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
               .HasConstraintName("fk_accumulated_room_id")
               .IsRequired();

        builder.HasOne(r => r.BotConfig)
               .WithOne(a => a.Room)
               .HasForeignKey<BotConfig>(a => a.RoomId)
               .HasConstraintName("fk_bot_config_room_id")
               .IsRequired();

        builder.HasOne(x => x.MediaAttachment)
            .WithOne()
            .HasForeignKey<MediaAttachment>(x => x.EntityId)
            .HasPrincipalKey<Room>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
