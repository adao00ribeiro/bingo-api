using bingo_api.src.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.src.Mappings;

public class MediaAttachmentMap : IEntityTypeConfiguration<MediaAttachment>
{
    public void Configure(EntityTypeBuilder<MediaAttachment> builder)
    {
        builder.ToTable("media_attachments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.ContentType).IsRequired();
        builder.Property(x => x.Size).IsRequired();

        builder.Property(x => x.EntityId).IsRequired();
        builder.Property(x => x.EntityType).IsRequired().HasMaxLength(100);

        // Índices para performance ⚡
        builder.HasIndex(x => new { x.EntityType, x.EntityId });
    }
}