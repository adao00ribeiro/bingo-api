using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record  ScratchGameAttributesDto
{
    public List<ScratchPayoutDto> PayoutTable { get; set; }

    internal static ScratchGameAttributesDto ConvertToDto(ScratchGameAttributes attributes)
    {
        return new ScratchGameAttributesDto
        {
            PayoutTable = [.. attributes.PayoutTable.Select(ScratchPayoutDto.ConvertToDto)],
        };
    }
}
