using bingo_api.src.DTOs.Response.Scratch.Jsonb;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response.Scratch;


public record ScratchGameResponseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public EScratchLayoutType? LayoutType { get; set; }
    public decimal? Price { get; set; }
    public decimal? MaxPrize { get; set; }
    public decimal? Probability { get; set; }
    public int[]? AllowedMultipliers { get; set; }
    public ScratchGameAttributesDto? Attributes { get; set; }

    internal static ScratchGameResponseDto ConvertToDto(ScratchGame entity)
    {
        return new ScratchGameResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            LayoutType = entity.LayoutType,
            Price = entity.Price,
            MaxPrize = entity.MaxPrize,
            Probability = entity.Probability,
            AllowedMultipliers = entity.AllowedMultipliers,
            Attributes = entity.Attributes is null
                ? null
                : ScratchGameAttributesDto.ConvertToDto(entity.Attributes)
        };
    }
}