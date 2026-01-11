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

    public async Task<Punter> GetByCpfAsync(string cpf)
    {
        var punter = await Context.Punters
           .Include(p => p.OnlineHouse)
               .ThenInclude(s => s.OwnerRooms)
           .Include(p => p.OnlineHouse)
               .ThenInclude(s => s.PaymentMethods)
          .FirstOrDefaultAsync(punter => punter.Cpf == cpf);
        return punter;
    }

    public async Task<Punter> GetByEmailAsync(string email)
    {
        var punter = await Context.Punters
             .Include(p => p.OnlineHouse)
                 .ThenInclude(s => s.OwnerRooms)
             .Include(p => p.OnlineHouse)
                 .ThenInclude(s => s.PaymentMethods)
            .FirstOrDefaultAsync(punter => punter.Email == email);

        return punter;
    }

    public async Task<Punter> GetByIdAsync(Guid id)
    {
        var punter = await Context.Punters
           .Include(p => p.OnlineHouse)
               .ThenInclude(s => s.OwnerRooms)
           .Include(p => p.OnlineHouse)
               .ThenInclude(s => s.PaymentMethods)
          .FirstOrDefaultAsync(punter => punter.Id == id);

        return punter;
    }

    public async Task<Punter> GetPunterByTag(String indicateTag)
    {
        return await Context.Punters.FirstOrDefaultAsync(punter => punter.IndicateTag == indicateTag); ;
    }

}
