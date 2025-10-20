using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;
using bingo_api.src.Extensions.Seeds;

namespace bingo_api.src.Entities.Scratch;

public class ScratchGame : Entity
{
    public string Name { get; set; }
    public EScratchLayoutType LayoutType { get; set; }
    public decimal Price { get; set; }
    public decimal MaxPrize { get; set; }
    public decimal? Probability { get; set; }
    public ScratchGameAttributes Attributes { get; set; } = new();

    // Stored as array in PostgreSQL (if using Npgsql)
    public int[] AllowedMultipliers { get; set; }
    // Navigation
    public IEnumerable<ScratchPrize> ScratchPrizes { get; set; }
    public IEnumerable<ScratchSellerGame> ScratchSellerGames { get; set; }

}
public class ScratchGameAttributes
{
    public List<ScratchPayout> PayoutTable { get; set; } = new();
    public List<ScratchSymbol> Symbols { get; set; } = new();
}

public class ScratchSymbol
{
    public string Name { get; set; }
    public string Symbol { get; set; } = "";
    public double Weight { get; set; }
    public double PrizeValue;
}