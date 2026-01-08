using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Providers;
using bingo_api.src.Services.Blockchain;

namespace bingo_api.src.Factory;

public class BlockchainServiceFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public BlockchainServiceFactory(ILoggerFactory loggerFactory)
    {

        _loggerFactory = loggerFactory;
    }

    public IBlockchainService Create(Network network, IEnumerable<TokenAddress> tokenAddresses)
    {
        if (network == null) throw new ArgumentNullException(nameof(network));
        if (tokenAddresses == null) throw new ArgumentNullException(nameof(tokenAddresses));
        Console.WriteLine("FDP" + network.Name.ToLower());
        switch (network.Name.ToLower())
        {
            case "ethereum":
            case "bsc":
            case "polygon":
            case "tenderly binance rialto":

                var provider = new EvmBlockchainProvider(network.RpcUrl);
                var logger = _loggerFactory.CreateLogger<EvmBlockchainService>();
                return new EvmBlockchainService(network.Name, provider, tokenAddresses.ToList(), logger);

            default:
                throw new NotSupportedException($"Rede {network.Name} não suportada.");
        }
    }
}
