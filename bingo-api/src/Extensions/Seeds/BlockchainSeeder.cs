
using bingo_api.src.Context;
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.Extensions.Seeds;

public class BlockchainSeeder: IDataSeeder
{
       private readonly DataContext _context;

    public BlockchainSeeder(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedNetworks();
        await SeedTokens();
        await SeedTokenAddresses();
    }

    private async Task SeedNetworks()
    {
        if (_context.BlockchainNetworks.Any())
            return;

        var bsc = new Network(
            "BNB Smart Chain Testnet",
            "https://data-seed-prebsc-1-s1.binance.org:8545",
            97
        );

        var tenderly = new Network(
            "Tenderly Binance Rialto",
            "https://virtual.binance-rialto.eu.rpc.tenderly.co/7d062b05-a250-47e7-af27-7ce855e8583a",
            1336
        );

        _context.BlockchainNetworks.AddRange(bsc, tenderly);
        await _context.SaveChangesAsync();
    }

    private async Task SeedTokens()
    {
        if (_context.BlockchainTokens.Any())
            return;

        var usdt = new Token("USDT", "USDT Testnet", 18);

        var tbnb = new Token(
            "TBNB",
            "",
            6,
            true
        );

        _context.BlockchainTokens.AddRange(usdt, tbnb);
        await _context.SaveChangesAsync();
    }

    private async Task SeedTokenAddresses()
    {
        if (_context.BlockchainTokenAddresss.Any())
            return;

        var bscId = _context.BlockchainNetworks.First(n => n.Name == "BNB Smart Chain Testnet").Id;
        var tenderlyId = _context.BlockchainNetworks.First(n => n.Name == "Tenderly Binance Rialto").Id;

        var USDTId = _context.BlockchainTokens.First(t => t.Symbol == "USDT").Id;
        var TBNBId = _context.BlockchainTokens.First(t => t.Symbol == "TBNB").Id;

        var usdtBsc = new TokenAddress(
            USDTId,
            bscId,
            "0x7ef95a0fee0b8ce4e3efbe19d7b2c349aef7f6f9"
        );

        var tbnbBsc = new TokenAddress(
            TBNBId,
            bscId,
            ""
        );

        var tbnbTenderly = new TokenAddress(
            TBNBId,
            tenderlyId,
            ""
        );

        _context.BlockchainTokenAddresss.AddRange(usdtBsc, tbnbBsc, tbnbTenderly);
        await _context.SaveChangesAsync();
    }
}
