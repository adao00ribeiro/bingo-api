
using System.Reflection;

using bingo_api.src.Entities;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Context;

public class DataContext : DbContext
{

    private readonly EventDispatcher _dispatcher;
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Punter> Punters { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomSeller> RoomSellers { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardBuy> CardBuys { get; set; }
    public DbSet<CardWinner> CardWinners { get; set; }
    public DbSet<Recharge> Recharges { get; set; }
    public DbSet<Accumulated> Accumulated { get; set; }
    public DbSet<BotConfig> BotConfigs { get; set; }
    public DbSet<TransactionHistory> TransactionHistories { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ScratchGame> ScratchGames { get; set; }
    public DbSet<ScratchSellerGame> ScratchSellerGames { get; set; }
    public DbSet<ScratchTicket> ScratchTickets { get; set; }
    public DbSet<ScratchPrize> ScratchPrizes { get; set; }
    public DbSet<Withdrawal> Withdrawals { get; set; }
    public DbSet<Network> BlockchainNetworks { get; set; }
    public DbSet<Token> BlockchainTokens { get; set; }
    public DbSet<TokenAddress> BlockchainTokenAddresss { get; set; }
    public DbSet<MediaAttachment> MediaAttachments { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DataContext(DbContextOptions<DataContext> options, EventDispatcher dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }
        
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
    

    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        foreach (var property in entityType.GetProperties())
        {
            if (property.ClrType == typeof(DateTime))
            {
                property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), // antes de salvar -> garante UTC
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)            // ao ler do banco -> marca como UTC
                ));
            }
            else if (property.ClrType == typeof(DateTime?))
            {
                property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime?, DateTime?>(
                    v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : v,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
                ));
            }
        }
    }
        base.OnModelCreating(modelBuilder);
    
        /*
        var allWithdrawals = await _context.Withdrawals.ToListAsync(); // Inclui ambos
        Buscar só de Seller:
        var sellerWithdrawals = await _context.Withdrawals
    .OfType<SellerWithdrawal>()
    .ToListAsync();*/
    }
    public async Task<int> SaveChangesWithoutEventsAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<Entity>()
        .Where(e => e.Entity.DomainEvents.Any())
        .Select(e => e.Entity)
        .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);
        // 🔔 Após salvar, processa eventos de domínio

        if (entities.Any())
            await _dispatcher.DispatchEventsAsync(entities);

        return result;
    }

}
