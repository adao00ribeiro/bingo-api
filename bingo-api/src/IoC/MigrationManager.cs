using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.IoC;

public static class MigrationManager
{
    public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TContext>();
                context.Database.Migrate(); // Aplica as migrations pendentes
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<TContext>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw; // Rethrow para garantir que erros críticos sejam tratados
            }
        }
        return host;
    }
}
