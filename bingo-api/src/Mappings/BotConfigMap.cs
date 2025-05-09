using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class BotConfigMap : IEntityTypeConfiguration<BotConfig>
{
     public void Configure(EntityTypeBuilder<BotConfig> builder)
    {

        builder.ToTable("BotConfigs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd();
        builder.Property(a => a.Enabled)
              .IsRequired();
        builder.Property(a => a.PresenceRate)
              .IsRequired();
        builder.HasOne(a => a.Room)
               .WithOne(r => r.BotConfig)
               .HasForeignKey<BotConfig>(a => a.RoomId)
               .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
    }
}
