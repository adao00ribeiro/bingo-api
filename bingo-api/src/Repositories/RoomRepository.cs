using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
namespace bingo_api.src.Repositories;

public class RoomRepository : RepositoryBase<Room>, IRoomRepository
{
    public RoomRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public override async Task<Guid> AddAsync(Room objeto)
    {
        Accumulated acumulated = new();
        objeto.Accumulated = acumulated;
        BotConfig config = new();
        objeto.BotConfig = config;
        return await base.AddAsync(objeto);
    }
}
