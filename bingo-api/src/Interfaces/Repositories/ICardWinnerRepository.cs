using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface ICardWinnerRepository : IRepositoryBase<CardWinner>
{
    Task<int> CountAsync(Guid guid);
}