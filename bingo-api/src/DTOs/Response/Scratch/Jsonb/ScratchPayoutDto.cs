using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchPayoutDto(double Multiplier , double Probability)
{
    internal static ScratchPayoutDto ConvertToDto(ScratchPayout payout)
    {
        return new ScratchPayoutDto(payout.Multiplier,payout.Probability);
    }
}
