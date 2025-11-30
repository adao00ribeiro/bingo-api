using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Interfaces.blockchain;
using Nethereum.Web3;

namespace bingo_api.src.Providers;

public class EvmBlockchainProvider : IBlockchainProvider
{
    private readonly string _rpcUrl;
    private readonly Web3 _web3;

    public EvmBlockchainProvider(string rpcUrl)
    {
        _rpcUrl = rpcUrl;
        _web3 = new Web3(rpcUrl);
    }

    public Web3 GetClient() => _web3;
    public string GetRpcUrl() => _rpcUrl;
}
