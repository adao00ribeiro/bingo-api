
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Context;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories;
using bingo_api.src.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bingo_api.src.Extensions;
using Hangfire;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;
using bingo_api.src.Jobs;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Jobs;
using Npgsql;


namespace bingo_api.src.IoC;

public static class NativeInjectorConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabasePostgreSQL"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<DataContext>(options =>
          {
              options.UseNpgsql(dataSource);
          }
        );
        services.AddDbContext<IdentityDataContext>(options =>
          options.UseNpgsql(configuration.GetConnectionString("DatabasePostgreSQL"))
      );

        services.AddDefaultIdentity<User>()
                          .AddRoles<IdentityRole>()
                          .AddEntityFrameworkStores<IdentityDataContext>()
                          .AddDefaultTokenProviders();

        services.AddHangfire(options =>
        {
            var connectionString = configuration.GetConnectionString("RedisConnection");
            var redis = ConnectionMultiplexer.Connect(connectionString);
            options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = $"HANG_FIRE" });

        });
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddScoped<DataInitializer>(); // Registrar o DataInitializer

        services.AddSingleton<IWebSocketService, WebSocketService>();

        //repository
        services.AddScoped<JwtSecurityExtensionEvents>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICardWinnerRepository, CardWinnerRepository>();
        services.AddScoped<IPrizeRepository, PrizeRepository>();
        services.AddScoped<IPunterRepository, PunterRepository>();
        services.AddScoped<IRechargeRepository, RechargeRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoundRepository, RoundRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IRoundRepository, RoundRepository>();
        services.AddScoped<ICardBuyRepository, CardBuyRepository>();
        services.AddScoped<IBotConfigRepository, BotConfigRepository>();
        services.AddScoped<IAccumulatedRepository, AccumulatedRepository>();
        services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();
        services.AddScoped<InsertBotRoundService>();

        //jobs
        services.AddScoped<IRoundFetcherJob, RoundFetcherJob>();
        services.AddScoped<IRoundExecutionJob, RoundExecutionJob>();
        services.AddScoped<IShowTimelineStepJob, ShowTimelineStepJob>();

        //services
        services.AddScoped<ICardBuyService, CardBuyService>();
        services.AddScoped<IDepositService, DepositService>();
        services.AddScoped<IReportService, ReportService>();


    }

}
