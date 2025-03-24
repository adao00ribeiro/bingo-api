using bingo_api.src.Interfaces.Jobs;
using bingo_api.src.Interfaces.Repositories;
using Hangfire;
using Hangfire.Server;

namespace bingo_api.src.Jobs;

public class RoundFetcherJob
{
    private readonly IRoundRepository _roundRepository;
    private readonly ILogger<RoundFetcherJob> _logger;

    public RoundFetcherJob(
        IRoundRepository roundRepository,
        ILogger<RoundFetcherJob> logger)
    {
        _roundRepository = roundRepository;
        _logger = logger;
    }
    public async Task Execute()
    {
        try
        {
            _logger.LogInformation("Iniciando Job1 - Processamento de todos os rounds");
            var now = DateTime.UtcNow;
            var futureTime = now.AddMinutes(50);
            int totalMinutes = (int)Math.Round(now.TimeOfDay.TotalMinutes / 10.0) * 10;
            var rounds = await _roundRepository.FilterByDateTimeRange(now.Date, TimeSpan.FromMinutes(totalMinutes), futureTime.TimeOfDay);
            _logger.LogInformation("total encontrado:" + rounds.Count());
            foreach (var round in rounds)
            {
                var jobId = BackgroundJob.Enqueue<RoundExecutionJob>(
                    job => job.Execute(round.Id)
                );

                _logger.LogInformation(
                    "Job2 enfileirado para o Round {RoundId}. JobId: {JobId}",
                    round.Id,
                    jobId
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no Job1 ao processar rounds");
            throw;
        }

    }

}
