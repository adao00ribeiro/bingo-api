using bingo_api.src.Context;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Blockchain;

public class NetworkRepository : RepositoryBase<Network>, INetworkRepository
{
    public NetworkRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public Task<Network?> GetByNetworkNameAsync(string NetwrokName)
    {
        return this.Context.BlockchainNetworks.FirstOrDefaultAsync(n => n.Name.ToLower() == NetwrokName.ToLower());
    }
}
