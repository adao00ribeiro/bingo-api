
using bingo_api.src.DTOs.Response.Scratch.Jsonb;

namespace bingo_api.src.DTOs.Request.Scratch;

public record ScratchGameRequestDto
{
    public string Name { get; init; }
    public int QuantityToAward { get; init; }
    public decimal Rtp { get; init; }
    public int Rows { get; init; }
    public int Cols { get; init; }
    public string Component { get; init; }

    public List<ScratchPayoutDto> PayoutTable { get; init; } = new();

    public int[] AllowedMultipliers { get; init; }
}
