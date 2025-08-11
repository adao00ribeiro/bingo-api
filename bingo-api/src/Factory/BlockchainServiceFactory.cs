using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Providers;
using bingo_api.src.Services.Blockchain;

namespace bingo_api.src.Factory;

public class BlockchainServiceFactory
{
    private readonly IConfiguration _config;
    private readonly ILoggerFactory _loggerFactory;

    public BlockchainServiceFactory(IConfiguration config, ILoggerFactory loggerFactory)
    {
        _config = config;
        _loggerFactory = loggerFactory;
    }

    public IBlockchainService Create(string networkName)
    {
        switch(networkName.ToLower())
        {
            case "ethereum":
            case "bsc":
            case "polygon":
                {
                    var rpcUri = _config.GetValue<string>($"Blockchain:RpcUris:{networkName}");
                    var tokensSection = _config.GetSection($"Blockchain:Tokens");
                    var tokenContracts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var tokenKey in tokensSection.GetChildren())
                    {
                        var address = tokenKey.GetValue<string>(networkName);
                        if (!string.IsNullOrEmpty(address))
                            tokenContracts[tokenKey.Key] = address;
                    }
                    var provider = new EvmBlockchainProvider(rpcUri);
                    var logger = _loggerFactory.CreateLogger<EvmBlockchainService>();
                    return new EvmBlockchainService(networkName, provider, tokenContracts, logger);
                }
                /*
            case "some-non-evm-network":
                {
                    // exemplo de não-EVM
                    return new NonEvmBlockchainService(networkName);
                }
                */
            default:
                throw new NotSupportedException($"Rede {networkName} não suportada.");
        }
    }
}
