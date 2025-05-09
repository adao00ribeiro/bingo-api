
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class AccumulatedRepository : RepositoryBase<Accumulated>, IAccumulatedRepository
{
    public AccumulatedRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<Accumulated> GetByRoomId(Guid roomId)
    {
           return await this.Context.Accumulated
        .Include(b => b.Room)
        .FirstOrDefaultAsync(b => b.RoomId == roomId);
    }
}
