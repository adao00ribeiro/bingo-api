using bingo_api.src.Interfaces.Jobs;
using Hangfire;

namespace bingo_api.src.Services;

public class MonitorService : IHostedService
{

    private readonly IServiceProvider _serviceProvider;


    public MonitorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await AddJobHangFire();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task AddJobHangFire()
    {

        using (var scope = _serviceProvider.CreateScope())
        {
            var _roundFetcherJob = scope.ServiceProvider.GetRequiredService<IRoundFetcherJob>();
            RecurringJob.AddOrUpdate("tarefa-cada-10-minutos", () =>

             _roundFetcherJob.Execute()

            , "*/10 * * * *");
        }

        // BackgroundJob.Enqueue( () => JobService.Execute());
        // BackgroundJob.Schedule(()=> JobService.Execute(),TimeSpan.FromSeconds(30));
        //  var parentId = BackgroundJob.Enqueue(() => JobService.Execute());
        //  BackgroundJob.ContinueJobWith(parentId,()=> JobService.Execute());
    }
}
