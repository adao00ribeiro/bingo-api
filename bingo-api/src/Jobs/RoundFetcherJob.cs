using bingo_api.src.Interfaces.Jobs;
using bingo_api.src.Interfaces.Repositories;
using Hangfire;

namespace bingo_api.src.Jobs;

public class RoundFetcherJob : IRoundFetcherJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebSocketService webSocketService;
    public RoundFetcherJob(IServiceProvider serviceProvider, IWebSocketService _webSocketService)
    {
        _serviceProvider = serviceProvider;
        webSocketService = _webSocketService;
    }
    public async Task Execute()
    {

        var now = DateTime.UtcNow;
        var futureTime = now.AddMinutes(50);
        using (var scope = _serviceProvider.CreateScope())
        {
            var roundRepository = scope.ServiceProvider.GetRequiredService<IRoundRepository>();

            var rounds = await roundRepository.FilterByDateTimeRange(now.Date, now.TimeOfDay, futureTime.TimeOfDay);
            if (rounds.Count() == 0)
            {
                Console.WriteLine("nenhum");
                Console.WriteLine(now.Date);

                Console.WriteLine(now);
                Console.WriteLine(futureTime);
            }
            var executionTasks = rounds.Select(round =>
                {
                    Console.WriteLine($"Iniciando execução do round {round.Id}");
                    var roundExecutionJob = scope.ServiceProvider.GetRequiredService<IRoundExecutionJob>();
                    return roundExecutionJob.Execute(round.Id)
                        .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Console.WriteLine($"Erro ao executar round {round.Id}: {task.Exception}");
                            }
                            else
                            {
                                Console.WriteLine($"Round {round.Id} executado com sucesso!");
                            }
                        });
                });
            await Task.WhenAll(executionTasks);
        }
    }
}
