using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IMediaAttachmentService
{
    Task RemoveAsync(MediaAttachment? attachment);
    Task<MediaAttachment> UploadOrUpdateAsync(
        MediaAttachment? existing,
        IFormFile newFile,
        Guid ownerId,
        string ownerType
    );
    Task<MediaAttachment> ApplyNewFileAsync(
        MediaAttachment? existingAttachment,
        IFormFile file,
        Guid ownerId,
        string ownerType
    );
}
