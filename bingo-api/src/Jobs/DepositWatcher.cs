using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Factory;

namespace bingo_api.src.Jobs;

    public class DepositWatcher : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<DepositWatcher> _logger;
        private readonly IConfiguration _config;
    private readonly IRechargeRepository _rechargeRepository;

    public DepositWatcher(IRechargeRepository rechargeRepository, IServiceProvider sp, ILogger<DepositWatcher> logger, IConfiguration config)
    {
        _sp = sp;
        _logger = logger;
        _config = config;
            _rechargeRepository = rechargeRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int interval = _config.GetValue<int>("DepositWatcher:IntervalSeconds", 30);

            _logger.LogInformation("DepositWatcher iniciado, intervalo {sec}s", interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IRechargeRepository>();
                    var factory = scope.ServiceProvider.GetRequiredService<BlockchainServiceFactory>();

                    var pendentes = await repo.GetAllAsync(filter: x => x.Network != "" && !x.IsConfirmed);
                    foreach (var dep in pendentes)
                    {
                        try
                        {
                            _logger.LogInformation("Verificando deposito Id={id} Tx={tx}", dep.Id, dep.TxHash);
                            var svc = factory.Create(dep.Network);
                            if (string.IsNullOrEmpty(dep.TxHash)) continue;

                            var ok = await svc.VerifyTransactionAsync(dep.TxHash!, dep.DestinationAddress, dep.Value, dep.Token);
                            if (ok)
                            {
                              await _rechargeRepository.UpdateStatusToCompleted(dep.Id, dep.Punter.Seller);
                                _logger.LogInformation("Deposito Id={id} confirmado", dep.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erro verificando deposito {id}", dep.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no loop do DepositWatcher");
                }

                await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
            }
        }
    }

