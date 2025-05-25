using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class SellerRepository : RepositoryBase<Seller>, ISellerRepository
{
    public SellerRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public async Task<Seller> GetByEmailAsync(string email)
    {
        return await Context.Sellers
           .FirstOrDefaultAsync(punter => punter.Email == email);
    }
}