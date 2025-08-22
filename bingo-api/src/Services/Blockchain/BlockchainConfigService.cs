using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;

namespace bingo_api.src.Services.Blockchain;

public class BlockchainConfigService: IBlockchainConfigService
{
    private readonly INetworkRepository _networkRepo;
    private readonly ITokenAddressRepository _tokenAddressRepo;

    public BlockchainConfigService(INetworkRepository networkRepo, ITokenAddressRepository tokenAddressRepo)
    {
        _networkRepo = networkRepo;
        _tokenAddressRepo = tokenAddressRepo;
    }

    public async Task<Network?> GetNetworkAsync(string networkName)
    {
        return await _networkRepo.GetByNetworkNameAsync(networkName);
    }

    public async Task<TokenAddress?> GetTokenAddressAsync(string networkName, string tokenName)
    {
        return await _tokenAddressRepo.GetByNetworkNameAndTokenSymbol(networkName,  tokenName);
         
    }
}