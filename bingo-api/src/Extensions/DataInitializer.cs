using bingo_api.src.Extensions.Seeds;


namespace bingo_api.src.Extensions;

public class DataInitializer
{
    private readonly IEnumerable<IDataSeeder> _seeders;

    public DataInitializer(IEnumerable<IDataSeeder> seeders)
    {
        _seeders = seeders;
    }

    public async Task SeedAsync()
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync();
        }
    }
}