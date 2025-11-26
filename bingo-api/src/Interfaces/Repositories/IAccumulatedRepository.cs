using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IAccumulatedRepository : IRepositoryBase<Accumulated>
{
    Task<Accumulated> GetByRoomId(Guid roomId);
    Task<Accumulated> UpdateAsync(Guid id, Accumulated accumulated);
}
