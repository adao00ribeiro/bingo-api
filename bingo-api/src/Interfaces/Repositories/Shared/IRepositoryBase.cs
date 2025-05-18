using System.Linq.Expressions;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Interfaces.Repositories.Shared;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync(int? pageNumber = null, int? pageSize = null, Expression<Func<TEntity, bool>> filter = null,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(TEntity objeto);
    Task UpdateAsync(TEntity objeto);
    Task RemoveAsync(TEntity objeto);
    Task RemoveByIdAsync(Guid id);
    Task<int> CountAsync();

}
