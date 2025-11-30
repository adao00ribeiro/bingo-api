using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Blockchain;

public class Token : Entity
{
    public string Symbol { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Decimals { get; set; }
    public bool IsNative { get; set; }
    public IEnumerable<TokenAddress> TokenAddresses { get; set; } = null!;

    public Token(string symbol, string name, int decimals, bool isNative = false)
    {
        Symbol = symbol;
        Name = name;
        Decimals = decimals;
        IsNative = isNative;
    }
}
