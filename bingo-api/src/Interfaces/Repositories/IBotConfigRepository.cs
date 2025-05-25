using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories;

public interface IBotConfigRepository : IRepositoryBase<BotConfig>
{
    Task<BotConfig> UpdateAsync(Guid id, BotConfig objeto);
    Task<BotConfig> CreateWithPuntersAsync(BotConfig botConfig);
    Task<BotConfig> GetByRoomId(Guid roomId);
}
