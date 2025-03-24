using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Repositories;

public interface IBotConfigRepository
{
    Task<BotConfig> CreateWithPuntersAsync(BotConfig botConfig);
}
