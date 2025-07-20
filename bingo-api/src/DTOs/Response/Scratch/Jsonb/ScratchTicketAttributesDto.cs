
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchTicketAttributesDto
{
    public Guid PunterId { get; set; }
    public List<ScratchItemDto> Items { get; set; } = new();

    internal static ScratchTicketAttributesDto ConvertToDto(ScratchTicketAttributes entity)
    {
        return new ScratchTicketAttributesDto
        {
            PunterId = entity.PunterId,
            Items = entity.Items?.Select(p=> ScratchItemDto.ConvertToDto(p)).ToList() ?? new List<ScratchItemDto>()
        };
    }
}