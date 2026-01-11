using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class MediaAttachmentRepository : RepositoryBase<MediaAttachment>, IMediaAttachmentRepository
{
    public MediaAttachmentRepository(DataContext dataContext) : base(dataContext)
    {
    }

   public async Task<List<MediaAttachment>> GetForAsync<TEntity>(
    IEnumerable<Guid> entityIds
)
{
    var entityType = typeof(TEntity).Name;

    return await Context.Set<MediaAttachment>()
        .Where(x =>
            entityIds.Contains(x.EntityId) &&
            x.EntityType == entityType
        )
        .ToListAsync();
}
}