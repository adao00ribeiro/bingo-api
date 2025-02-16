using System.Linq.Expressions;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class CardRepository : RepositoryBase<Card>, ICardRepository
{
    public CardRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task AddRangeAsync(List<Card> cardsToInsert)
    {
        await Context.Cards.AddRangeAsync(cardsToInsert);
        Context.SaveChanges();
    }

    public async Task<IEnumerable<Card>> GetAllByRoundId(Guid punterId, Guid roundId,params Expression<Func<Card, object>>[] includeProperties)
    {

        // Cria a consulta base com os Includes passados
    IQueryable<Card> query = base.BuildQueryWithIncludes(includeProperties);

    // Filtra pela combinação de punterId e roundId
    query = query.Where(card => card.PunterId == punterId && card.RoundId == roundId);

    // Retorna os resultados de forma assíncrona com o AsNoTracking (para melhorar a performance de leitura)
    return await query.AsNoTracking().ToListAsync();
    }
}
