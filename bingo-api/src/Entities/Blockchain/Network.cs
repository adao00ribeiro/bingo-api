using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Blockchain;

public class Network : Entity
{
    public string Name { get; set; } = null!;
    public string RpcUrl { get; set; } = null!;
    public int ChainId { get; set; }
    public IEnumerable<TokenAddress> TokenAddresses { get; set; } = null!;

    public Network(string name, string rpcUrl, int chainId) 
    {
        Name = name;
        RpcUrl = rpcUrl;
        ChainId = chainId;
    }
}
