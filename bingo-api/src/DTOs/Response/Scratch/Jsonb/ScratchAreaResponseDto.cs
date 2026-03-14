using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchAreaResponseDto
{
    public int Element { get; set; }
    public DateTime? ScratchedAt { get; set; }

   public static ScratchAreaResponseDto ConvertToDto(ScratchArea entity)
    {
        return new ScratchAreaResponseDto
        {
            Element = entity.Element,
            ScratchedAt = entity.ScratchedAt,
        };
    }
}
