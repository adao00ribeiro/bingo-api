using System.Linq.Expressions;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface ICardRepository : IRepositoryBase<Card>
{
    Task AddRangeAsync(List<Card> cardsToInsert);
    Task<IEnumerable<Card>> GetAllByRoundId(Guid punterId, Guid roundId, int? page = null, int? size = null, params Expression<Func<Card, object>>[] includeProperties);
}