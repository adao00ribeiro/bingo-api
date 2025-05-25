using bingo_api.src.Context;
using bingo_api.src.DTOs.Request;
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

    public async Task<IEnumerable<Round>> FilterByRoomIdAsync(Guid roomId, Guid punterId)
    {
        DateTime currentDateTime = DateTime.UtcNow;
        var startTime = currentDateTime.AddMinutes(-10);
        var endTime = startTime.AddHours(24);

        // Busca os rounds com os filtros
        var rounds = await Context.Rounds
            .Where(r => r.RoomId == roomId &&
                        r.Started >= startTime &&
                        r.Started <= endTime &&
                        r.Finished == null)
            .OrderBy(r => r.Started)
            .AsNoTracking()
            .ToListAsync();

        var roundIds = rounds.Select(r => r.Id).ToList();

        // Busca os cards comprados pelo punter para esses rounds
        var cardsByRound = await Context.Cards
            .Where(c => roundIds.Contains(c.RoundId) && c.PunterId == punterId)
            .GroupBy(c => c.RoundId)
            .Select(g => new { RoundId = g.Key, Count = g.Count() })
            .ToListAsync();

        // Mapeia o número de cards para cada round
        foreach (var round in rounds)
        {
            round.CardsPurchased = cardsByRound
                .FirstOrDefault(c => c.RoundId == round.Id)?.Count ?? 0;
        }

        // Carrega os prêmios associados aos rounds
        var prizes = await Context.Prizes
            .Where(p => roundIds.Contains(p.RoundId))
            .ToListAsync();

        foreach (var round in rounds)
        {
            round.Prizes = prizes.Where(p => p.RoundId == round.Id).ToList();
        }

        return rounds;
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
            var startTimeLocal = date.ToDateTime(request.StartedTime); // Assume que está no fuso local

            var endTimeLocal = date.ToDateTime(request.FinishedTime);
            var startTimeUtc = TimeZoneInfo.ConvertTimeToUtc(startTimeLocal, TimeZoneInfo.Local);
            var endTimeUtc = TimeZoneInfo.ConvertTimeToUtc(endTimeLocal, TimeZoneInfo.Local);
            var currentTime = startTimeUtc;
            while (currentTime <= endTimeUtc)
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

    public async Task<ICollection<Prize>> GetPrizes(Guid roundId)
    {

        var prizes = await Context.Prizes
             .Where(p => p.RoundId == roundId)
                             .ToListAsync();
        return prizes;
    }

    public async Task RemoveCards(Guid RoundId)
    {
        var cardsToRemove = Context.Cards
            .Where(card => card.RoundId == RoundId &&
                           !Context.CardWinners.Any(winner => winner.CardId == card.Id));
       Context.Cards.RemoveRange(cardsToRemove);
       await Context.SaveChangesAsync();
    }
}