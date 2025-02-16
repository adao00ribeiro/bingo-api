using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IRoundRepository : IRepositoryBase<Round>
{
    Task<IEnumerable<Round>> FilterByDateTimeRange(DateTime today, TimeSpan timeOfDay1, TimeSpan timeOfDay2);
}