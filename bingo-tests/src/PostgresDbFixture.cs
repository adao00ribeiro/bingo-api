
using bingo_api.src.Context;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace bingo_tests.src;

public class PostgresDbFixture : IDisposable
{
    public DataContext Context { get; }
    public IServiceProvider ServiceProvider { get; }

    public PostgresDbFixture()
    {
        var services = new ServiceCollection();

        // Configura o DbContext com PostgreSQL
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5433;Database=test-db;Username=postgres;Password=bingo123456"));

        // Registra os repositórios
        services.AddScoped<ISellerRepository, SellerRepository>();

        // Constrói o provider
        ServiceProvider = services.BuildServiceProvider();

        // Pega o contexto criado pelo provider
        Context = ServiceProvider.GetRequiredService<DataContext>();

        // Garante banco limpo
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        //Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
