using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchSymbolDto
{
    public string Symbol { get; set; }
    public decimal PrizeValue { get; set; }

    internal static ScratchSymbolDto ConvertToDto(ScratchSymbol symbol)
    {
        return new ScratchSymbolDto
        {
            Symbol = symbol.Symbol,
            PrizeValue = symbol.PrizeValue
        };
    }
}