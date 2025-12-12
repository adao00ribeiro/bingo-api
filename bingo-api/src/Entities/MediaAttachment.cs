using bingo_api.src.Domain.Events;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class MediaAttachment: Entity
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = null!;

    public MediaAttachment(string fileName, string url , string contentType, long size, Guid entityId, string entityType)
    {
        FileName = fileName;
        Url = url;
        ContentType = contentType;
        Size = size;
        EntityId = entityId;
        EntityType = entityType;
    }
}
