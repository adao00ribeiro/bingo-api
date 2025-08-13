
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
using bingo_api.src.Adapter;
using bingo_api.src.Interceptors;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Factory;
using bingo_api.src.Repositories.Blockchain;


namespace bingo_api.src.IoC;

public static class NativeInjectorConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabasePostgreSQL"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<DataContext>((provider, options) =>
          {
              var interceptor = provider.GetRequiredService<BalanceChangeInterceptor>();
              options.UseNpgsql(dataSource);
              options.AddInterceptors(interceptor);
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
        services.AddScoped<BalanceChangeInterceptor>();
        services.AddScoped<DataInitializer>(); // Registrar o DataInitializer

        services.AddSingleton<IWebSocketService, WebSocketService>();

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));
        services.AddSingleton<BlockchainServiceFactory>();
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
        services.AddScoped<INetworkRepository, NetworkRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITokenAddressRepository, TokenAddressRepository>();
    
        services.AddScoped<InsertBotRoundService>();

        //jobs
        services.AddScoped<IRoundFetcherJob, RoundFetcherJob>();
        services.AddScoped<IRoundExecutionJob, RoundExecutionJob>();
        services.AddScoped<IShowTimelineStepJob, ShowTimelineStepJob>();

        //services
        services.AddScoped<ICardBuyService, CardBuyService>();
        services.AddScoped<IPaymentProvider, PixManualAdapter>();
        services.AddHttpClient<PushPayAdapter>();
        services.AddHttpClient<TelegamNotifierService>();
        services.AddScoped<IPaymentProvider, PushPayAdapter>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IWithdrawalService, WithdrawalService>();



    }

}
