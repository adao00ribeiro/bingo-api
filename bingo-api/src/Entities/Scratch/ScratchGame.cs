using bingo_api.src.Entities.Shared;
using bingo_api.src.Extensions.Seeds;

namespace bingo_api.src.Entities.Scratch;

public class ScratchGame : Entity
{
    public string Name { get; set; }
    public int QuantityToAward { get; set; } 
    public int Rows { get; set; }
    public int Cols { get; set; }
    public decimal Rtp { get; set; }
    public string Component { get; set; }
    public ScratchGameAttributes Attributes { get; set; } = new();

    // Stored as array in PostgreSQL (if using Npgsql)
    public int[] AllowedMultipliers { get; set; }
    // Navigation
    public IEnumerable<ScratchSellerGame> ScratchSellerGames { get; set; }

}
public class ScratchGameAttributes
{
    public List<ScratchPayout> PayoutTable { get; set; } = new();
}