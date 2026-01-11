using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IMediaAttachmentRepository : IRepositoryBase<MediaAttachment>
{
    Task<List<MediaAttachment>> GetForAsync<TEntity>(IEnumerable<Guid> entityIds);
}
