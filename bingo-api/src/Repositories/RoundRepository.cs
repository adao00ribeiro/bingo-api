using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class RoundRepository : RepositoryBase<Round>, IRoundRepository
{

    public RoundRepository(DataContext dataContext) : base(dataContext)
    {
    }


    public override Task<Guid> AddAsync(Round objeto)
    {
        if (objeto.Prizes?.Count() == 0)
        {
            objeto.AddPrize(new Prize(10, EPrizeType.FourInLine));
        }
        return base.AddAsync(objeto);
    }
    public async Task<IEnumerable<Round>> FilterByDateTimeRange(DateTime today, TimeSpan timeOfDay1, TimeSpan timeOfDay2)
    {
        Console.WriteLine("FILTRO BEST TESTE");
        Console.WriteLine(today);
        Console.WriteLine(timeOfDay1);
        Console.WriteLine(timeOfDay2);

        return await Context.Rounds
            .Where(round => round.Started.Date == today &&
                            round.Started.TimeOfDay >= timeOfDay1 &&
                             round.Started.TimeOfDay <= timeOfDay2
                            )
                            .ToListAsync();
    }
}