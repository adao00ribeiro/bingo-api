using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class MediaAttachmentService(IMediaAttachmentRepository repo ,  MinioFileService minioFileService) : IMediaAttachmentService
{
    private readonly IMediaAttachmentRepository _repo = repo;
    private readonly MinioFileService _minioFileService = minioFileService;

  /// <summary>
    /// Remove o arquivo do MinIO e remove a entidade do banco (se existir).
    /// </summary>
    public async Task RemoveAsync(MediaAttachment? attachment)
    {
        if (attachment == null)
            return;

        // Tenta remover do MinIO (ignora se já não existir)
        if (!string.IsNullOrWhiteSpace(attachment.Url))
        {
            try
            {
                await _minioFileService.DeleteAsync(attachment.Url);
            }
            catch
            {
                // opcional: log aqui; normalmente não queremos falhar a remoção do DB por um erro no MinIO
            }
        }

        // Remove a entidade do DB
        await _repo.RemoveAsync(attachment);
    }

    /// <summary>
    /// Faz upload do arquivo para o MinIO e atualiza ou cria a entidade MediaAttachment no banco.
    /// Retorna a entidade persistida (com Id).
    /// </summary>
    public async Task<MediaAttachment> UploadOrUpdateAsync(
        MediaAttachment? existing,
        IFormFile newFile,
        Guid ownerId,
        string ownerType
    )
    {
        // Gera nome único para MinIO
        string fileName = $"{ownerId}_{newFile.FileName}";

        // Upload para MinIO -> retorna a URL (ou o key dependendo da sua implementação)
        string url = await _minioFileService.UploadAsync(newFile, fileName);

        if (existing != null)
        {
            // Atualiza campos da entidade existente (mantendo tracking se vier do contexto)
            existing.FileName = newFile.FileName;
            existing.Url = url;
            existing.ContentType = newFile.ContentType;
            existing.Size = newFile.Length;
            existing.UpdatedAt = DateTime.UtcNow; // se tiver coluna de update

            await _repo.UpdateAsync(existing);

            return existing;
        }

        // Cria nova entidade e persiste
        var media = new MediaAttachment(
            fileName: newFile.FileName,
            url: url,
            contentType: newFile.ContentType,
            size: newFile.Length,
            entityId: ownerId,
            entityType: ownerType
        );

        await _repo.AddAsync(media);

        return media;
    }

    /// <summary>
    /// Fluxo completo: remove attachment antigo (MinIO + DB), faz upload do novo arquivo e persiste a entidade.
    /// Retorna a entidade nova/atualizada.
    /// </summary>
    public async Task<MediaAttachment> ApplyNewFileAsync(
        MediaAttachment? existingAttachment,
        IFormFile file,
        Guid ownerId,
        string ownerType
    )
    {
        // Remove antigo (se existir) - isso já remove do MinIO e do DB
        if (existingAttachment != null)
        {
            await RemoveAsync(existingAttachment);
        }

        // Faz upload e cria a entidade no DB
        var newMedia = await UploadOrUpdateAsync(null, file, ownerId, ownerType);
        return newMedia;
    }
}
