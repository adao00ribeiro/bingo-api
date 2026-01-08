using bingo_api.src.Context;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Blockchain;

public class TokenAddressRepository : RepositoryBase<TokenAddress>, ITokenAddressRepository
{
    public TokenAddressRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<TokenAddress?> GetByNetworkNameAndTokenSymbol(string networkName, string tokenName)
    {

        return this.Context.BlockchainTokenAddresss
        .Include(t => t.Network)
        .Include(t => t.Token)
        .FirstOrDefault(t => t.Network.Name.ToLower() == networkName.ToLower()
                          && t.Token.Name.ToLower() == tokenName.ToLower());
    }
}
