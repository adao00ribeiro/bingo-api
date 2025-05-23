using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Request.Report;
using bingo_api.src.DTOs.Response.report;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IRoundRepository : IRepositoryBase<Round>
{
    Task<IEnumerable<Round>> FilterByDateTimeRange(DateTime today, TimeSpan timeOfDay1, TimeSpan timeOfDay2);
    Task<IEnumerable<Round>> FilterByRoomIdAsync(Guid roomId, Guid PunterId);
    Task<bool> GenerateRounds(RoundBulkRequestDto request);
    Task<ICollection<Prize>> GetPrizes(Guid roundId);
    Task RemoveCards(Guid RoundId);
}