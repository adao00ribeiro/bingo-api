using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.Interfaces.blockchain;

public interface IBlockchainConfigService
{
    Task<Network?> GetNetworkAsync(string networkName);
    Task<TokenAddress?> GetTokenAddressAsync(string networkName, string tokenSymbol);

}
