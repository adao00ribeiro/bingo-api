using bingo_api.src.Extensions.Seeds;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchPayoutDto(decimal Multiplier, decimal Prize)
{
    internal static ScratchPayoutDto ConvertToDto(ScratchPayout payout)
    {
        return new ScratchPayoutDto(payout.Multiplier, payout.Prize);
    }
}
