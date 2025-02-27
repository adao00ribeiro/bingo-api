using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace bingo_api.src.Repositories;

public class RoundRepository : RepositoryBase<Round>, IRoundRepository
{

    public RoundRepository(DataContext dataContext) : base(dataContext)
    {
    }

public async Task<IEnumerable<Round>> FilterByRoomIdAsync(
    Guid roomId, DateTime date, TimeSpan startTime, TimeSpan endTime, Guid PunterId)
{
    var query = @"SELECT r.*, COUNT(c.""Id"") AS CardsPurchased
    FROM ""Rounds"" r
    LEFT JOIN ""Cards"" c ON c.""RoundId"" = r.""Id"" AND c.""PunterId"" = @PunterId
    WHERE r.""RoomId"" = @RoomId
    AND DATE(r.""Started"") = @Date
    AND CAST(r.""Started"" AS TIME) BETWEEN @StartTime AND @EndTime
    AND r.""Finished"" IS NULL
    GROUP BY r.""Id""";

    return await Context.Rounds
        .FromSqlRaw(query, 
            new NpgsqlParameter("@RoomId", roomId),
            new NpgsqlParameter("@Date", date),
            new NpgsqlParameter("@StartTime", startTime),
            new NpgsqlParameter("@EndTime", endTime),
            new NpgsqlParameter("@PunterId", PunterId)
        ).ToListAsync();
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