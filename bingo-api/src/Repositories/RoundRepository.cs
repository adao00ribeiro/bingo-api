using bingo_api.src.Context;
using bingo_api.src.DTOs.Request;
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
        Guid roomId, Guid PunterId)
    {
    DateTime currentDateTime = DateTime.UtcNow;
    // Calculando startTime (10 minutos antes do tempo atual)
    var startTime = currentDateTime.AddMinutes(-10);
    // Calculando endTime (24 horas depois do startTime)
    var endTime = startTime.AddHours(24);
    //TimeSpan endTime = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59); 
    Console.WriteLine("opa" +roomId);

    Console.WriteLine("opa" +PunterId);

    Console.WriteLine("opa" +currentDateTime.Date);
    Console.WriteLine("opa" +startTime);
    Console.WriteLine("opa" +endTime);




        var query = @"SELECT r.*, COUNT(c.""Id"") AS CardsPurchased
    FROM ""Rounds"" r
    LEFT JOIN ""Cards"" c ON c.""RoundId"" = r.""Id"" AND c.""PunterId"" = @PunterId
    WHERE r.""RoomId"" = @RoomId
    AND r.""Started"" >= @StartTime 
    AND r.""Started"" <= @EndTime
    AND r.""Finished"" IS NULL
    GROUP BY r.""Id""";

        return await Context.Rounds
            .FromSqlRaw(query,
                new NpgsqlParameter("@RoomId", roomId),
                new NpgsqlParameter("@Date", currentDateTime.Date),
                new NpgsqlParameter("@StartTime", startTime),
                new NpgsqlParameter("@EndTime", endTime),
                new NpgsqlParameter("@PunterId", PunterId)
            ).ToListAsync();
    }
    public override Task<Guid> AddAsync(Round objeto)
    {
        if (objeto.Prizes?.Count == 0)
        {
            objeto.AddPrize(new Prize(10, EPrizeType.FourInLine));
            objeto.AddPrize(new Prize(20, EPrizeType.SingleLine));
            objeto.AddPrize(new Prize(30, EPrizeType.FullCard));
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
       public async Task<bool> GenerateRounds(RoundBulkRequestDto request)
    {
        var roundsToInsert = GenerateRoundsList(request);

        await Context.Rounds.AddRangeAsync(roundsToInsert);
        await Context.SaveChangesAsync();

        var prizesToInsert = GeneratePrizesList(request, roundsToInsert);
        await Context.Prizes.AddRangeAsync(prizesToInsert);
        await Context.SaveChangesAsync();
        return true;
    }

    private List<Round> GenerateRoundsList(RoundBulkRequestDto request)
    {
        var rounds = new List<Round>();

        for (var date = request.StartedDate; date <= request.FinishedDate; date = date.AddDays(1))
        {
            var startTime = DateTime.SpecifyKind(date.ToDateTime(request.StartedTime), DateTimeKind.Utc);
            var endTime = DateTime.SpecifyKind(date.ToDateTime(request.FinishedTime), DateTimeKind.Utc);
            var currentTime = startTime;
            while (currentTime <= endTime)
            {
                rounds.Add(new Round(request.CardValue, currentTime, request.TimeBetweenBalls, request.MaxBalls, request.CardRows, request.CardColumns, request.RoomId));
                currentTime = currentTime.AddMinutes(request.TimeBetweenRounds);
            }
        }
        return rounds;
    }

    private List<Prize> GeneratePrizesList(RoundBulkRequestDto request, List<Round> rounds)
    {
        return rounds.SelectMany(round => request.Prizes.Select(prize => new Prize(prize.Value, prize.Type, round.Id)
        )).ToList();
    }
}