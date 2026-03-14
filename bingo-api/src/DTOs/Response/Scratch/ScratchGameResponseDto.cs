using bingo_api.src.DTOs.Response.Scratch.Jsonb;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response.Scratch;


public record ScratchGameResponseDto
{
      public Guid Id { get; set; }

    public string Name { get; set; }

    public int QuantityToAward { get; set; }

    public double Rtp { get; set; }

    public int Rows { get; set; }

    public int Cols { get; set; }

    public string Component { get; set; }

    public ScratchGameAttributesDto Attributes { get; set; } = new();

    public int[] AllowedMultipliers { get; set; }

    public static ScratchGameResponseDto ConvertToDto(ScratchGame entity)
    {
        return new ScratchGameResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            QuantityToAward = entity.QuantityToAward,
            Rtp = entity.Rtp,
            Rows = entity.Rows,
            Cols = entity.Cols,
            Component = entity.Component,
            AllowedMultipliers = entity.AllowedMultipliers,
            Attributes = entity.Attributes is null
                ? null
                : ScratchGameAttributesDto.ConvertToDto(entity.Attributes)
        };
    }
}