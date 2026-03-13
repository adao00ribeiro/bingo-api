using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchBuyResponseDto
{
     public Guid Id { get; set; }

    public decimal Value { get; set; }

    public int Quantity { get; set; }

    public Guid ScratchGameOverrideId { get; set; }

    public Guid PunterId { get; set; }

    public static ScratchBuyResponseDto ConvertToDto(ScratchBuy entity)
    {
        return new ScratchBuyResponseDto
        {
            Id = entity.Id,
            Value = entity.Value,
            Quantity = entity.Quantity,
            ScratchGameOverrideId = entity.ScratchGameOverrideId,
            PunterId = entity.PunterId,
        };
    }
}
