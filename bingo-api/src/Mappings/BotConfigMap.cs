using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class BotConfigMap : IEntityTypeConfiguration<BotConfig>
{
    public void Configure(EntityTypeBuilder<BotConfig> builder)
    {
        builder.ToTable("bot_configs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Enabled)
               .HasColumnName("enabled")
               .IsRequired();

        builder.Property(a => a.PresenceRate)
               .HasColumnName("presence_rate")
               .IsRequired();

        builder.Property(a => a.RoomId)
               .HasColumnName("room_id");
        builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(a => a.Room)
               .WithOne(r => r.BotConfig)
               .HasForeignKey<BotConfig>(a => a.RoomId)
               .HasConstraintName("fk_bot_config_room_id")
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
    }
}
