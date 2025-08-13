using bingo_api.src.Context;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories.Blockchain;

public class TokenAddressRepository : RepositoryBase<TokenAddress>, ITokenAddressRepository
{
    public TokenAddressRepository(DataContext dataContext) : base(dataContext)
    {
    }
}
