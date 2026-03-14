using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchTicketAttributesDto
{
    public List<ScratchAreaResponseDto> Areas { get; set; } = new();

    internal static ScratchTicketAttributesDto ConvertToDto(ScratchTicketAttributes entity)
    {
        return new ScratchTicketAttributesDto
        {
            Areas = entity.Areas?.Select(p => ScratchAreaResponseDto.ConvertToDto(p)).ToList() ?? new List<ScratchAreaResponseDto>()
        };
    }
}
