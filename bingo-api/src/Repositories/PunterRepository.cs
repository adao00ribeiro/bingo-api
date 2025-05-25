using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class PunterRepository : RepositoryBase<Punter>, IPunterRepository
{
    public PunterRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<Punter> GetByEmailAsync(string email)
    {
        var punter = await Context.Punters
             .Include(p => p.Seller)
                 .ThenInclude(s => s.OwnerRooms) 
            .FirstOrDefaultAsync(punter => punter.Email == email);

        return punter;
    }

}
