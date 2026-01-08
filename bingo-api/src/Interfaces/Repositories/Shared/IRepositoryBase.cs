using System.Linq.Expressions;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Interfaces.Repositories.Shared;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync(int? pageNumber = null, int? pageSize = null, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeProperties = null);
    Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeProperties = null);
    Task<Guid> AddAsync(TEntity objeto);
    Task UpdateAsync(TEntity objeto);
    Task UpdatePartialAsync(Guid Id, Dictionary<string, object?> updates);
    Task RemoveAsync(TEntity objeto);
    Task RemoveByIdAsync(Guid id);
    Task<int> CountAsync();

}
