using bingo_api.src.Jobs;
using Hangfire;
namespace bingo_api.src.Extensions;

public static class HangfireJobsExtension
{
    public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app)
    {
        var recurringJobManager = app.ApplicationServices
         .GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<RoundFetcherJob>(
               "process-rounds-job",
               job => job.Execute(),
               "*/10 * * * *"
           );

        return app;
    }
}
