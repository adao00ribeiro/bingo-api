using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.blockchain;
using Microsoft.EntityFrameworkCore;
using bingo_api.src.Factory;

namespace bingo_api.src.Jobs;

public class DepositWatcher : BackgroundService
{
    private readonly ILogger<DepositWatcher> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;

    public DepositWatcher(
        ILogger<DepositWatcher> logger,
        IConfiguration config,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _config = config;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int interval = _config.GetValue<int>("DepositWatcher:IntervalSeconds", 30);
        _logger.LogInformation("DepositWatcher iniciado, intervalo {sec}s", interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                // Resolva os serviços scoped dentro do escopo
                var rechargeRepository = scope.ServiceProvider.GetRequiredService<IRechargeRepository>();
                var blockchainServiceFactory = scope.ServiceProvider.GetRequiredService<BlockchainServiceFactory>();
                var blockchainConfigService = scope.ServiceProvider.GetRequiredService<IBlockchainConfigService>();

                var pendentes = await rechargeRepository.GetAllAsync(
                    filter: x => x.ConfirmedAt == null && x.DestinationAddress != null,
                    includeProperties: r => r.Include(x => x.Punter).ThenInclude(x => x.Seller)
                );

                if (!pendentes.Any())
                {
                    _logger.LogInformation("Nenhum depósito Crypto encontrado");
                }

                foreach (var dep in pendentes)
                {
                    try
                    {
                        _logger.LogInformation("Verificando depósito Id={id} Tx={tx}", dep.Id, dep.TxHash);

                        if (string.IsNullOrEmpty(dep.TxHash) || string.IsNullOrEmpty(dep.Network) || string.IsNullOrEmpty(dep.Token))
                        {
                            _logger.LogWarning("Depósito incompleto Id={id}: Network ou Token ausente", dep.Id);
                            continue;
                        }

                        var network = await blockchainConfigService.GetNetworkAsync(dep.Network)
                            ?? throw new InvalidOperationException($"Network {dep.Network} não configurada");

                        var tokenAddress = await blockchainConfigService.GetTokenAddressAsync(dep.Network, dep.Token)
                            ?? throw new InvalidOperationException($"Token {dep.Token} não configurado para rede {dep.Network}");

                        var svc = blockchainServiceFactory.Create(network, new List<TokenAddress> { tokenAddress });

                        var ok = await svc.VerifyTransactionAsync(dep.TxHash, dep.DestinationAddress!, dep.Amount, dep.Token);
                        if (ok)
                        {
                            await rechargeRepository.UpdateStatusToCompleted(dep.Id, dep.Punter.Seller);
                            _logger.LogInformation("Depósito Id={id} confirmado", dep.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro verificando depósito Id={id}", dep.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no DepositWatcher");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // shutdown solicitado
            }
        }
    }
}
