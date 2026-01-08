using bingo_api.src.Entities;
using bingo_api.src.Services;

namespace bingo_api.src.DTOs.Response;

public record MediaAttachmentResponseDto
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = null!;

    public static MediaAttachmentResponseDto? ConvertToDto(MediaAttachment? media)
    {
        if (media == null) return null;

        return new MediaAttachmentResponseDto
        {
            FileName = media.FileName,
            Url = media.Url,
            ContentType = media.ContentType,
            Size = media.Size,
            EntityId = media.EntityId,
            EntityType = media.EntityType
        };
    }
}
