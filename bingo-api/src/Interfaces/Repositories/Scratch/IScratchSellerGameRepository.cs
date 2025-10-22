using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories.Scratch;

public interface IScratchSellerGameRepository: IRepositoryBase<ScratchSellerGame>
{
    Task<int> CountAsync(Guid ownerId);
    Task<ScratchSellerGame?> GetByIdAsync(Guid id);
}