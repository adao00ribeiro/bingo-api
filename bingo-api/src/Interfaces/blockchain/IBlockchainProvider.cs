using Nethereum.Web3;

namespace bingo_api.src.Interfaces.blockchain;

public interface IBlockchainProvider
{
    Web3 GetClient();
    public string GetRpcUrl();
}
