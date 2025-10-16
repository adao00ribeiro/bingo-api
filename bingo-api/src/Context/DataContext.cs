
using System.Reflection;

using bingo_api.src.Entities;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Entities.Shared;
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
    public DbSet<CardBuy> CardBuys { get; set; }
    public DbSet<CardWinner> CardWinners { get; set; }
    public DbSet<Recharge> Recharges { get; set; }
    public DbSet<Accumulated> Accumulated { get; set; }
    public DbSet<BotConfig> BotConfigs { get; set; }
    public DbSet<TransactionHistory> TransactionHistories { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ScratchGame> ScratchGames { get; set; }
    public DbSet<ScratchTicket> ScratchTickets { get; set; }
    public DbSet<Withdrawal> Withdrawals { get; set; }
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
 modelBuilder.Entity<Withdrawal>()
        .HasDiscriminator<string>("withdrawal_type")
        .HasValue<PunterWithdrawal>("Punter")
        .HasValue<SellerWithdrawal>("Seller");
        
    modelBuilder.Entity<PunterWithdrawal>()
        .HasOne(pw => pw.Punter)
        .WithMany(p => p.Withdrawals)
        .HasForeignKey(pw => pw.PunterId);

    modelBuilder.Entity<SellerWithdrawal>()
        .HasOne(sw => sw.Seller)
        .WithMany(s => s.Withdrawals)
        .HasForeignKey(sw => sw.SellerId);


        base.OnModelCreating(modelBuilder);

        /*
        var allWithdrawals = await _context.Withdrawals.ToListAsync(); // Inclui ambos
        Buscar só de Seller:
        var sellerWithdrawals = await _context.Withdrawals
    .OfType<SellerWithdrawal>()
    .ToListAsync();*/
    }

}
