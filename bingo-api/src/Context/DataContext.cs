
using System.Reflection;

using bingo_api.src.Entities;
using bingo_api.src.Structs;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Context;

public class DataContext : DbContext
{
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Punter> Punters { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomSeller> RoomSellers { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardWinner> CardWinners { get; set; }
    public DbSet<Recharge> Recharges { get; set; }
    public DbSet<Accumulated> Accumulated { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
        .AreUnicode(false).HaveMaxLength(500);
        base.ConfigureConventions(configurationBuilder);

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly("bingo_api.src.Mappings");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

    }

}
