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

    public async Task<IEnumerable<Card>> GetAllByRoundId(Guid punterId, Guid roundId, int? page = null, int? size = null, Func<IQueryable<Card>, IQueryable<Card>>? includeProperties = null)
    {
        // Cria a consulta base com os Includes passados
        IQueryable<Card> query = base.BuildQueryWithIncludes(includeProperties);
        query = query.Where(card => card.PunterId == punterId && card.RoundId == roundId);
        if (page.HasValue && size.HasValue)
        {
            query = query.Skip((page.Value - 1) * size.Value).Take(size.Value);
        }
        // Filtra pela combinação de punterId e roundId

        // Retorna os resultados de forma assíncrona com o AsNoTracking (para melhorar a performance de leitura)
        return await query.AsNoTracking().ToListAsync();
    }
}
