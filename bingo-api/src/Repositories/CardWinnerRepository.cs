using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories;

public class CardWinnerRepository : RepositoryBase<CardWinner>, ICardWinnerRepository
{
    public CardWinnerRepository(DataContext dataContext) : base(dataContext)
    {
    }


}