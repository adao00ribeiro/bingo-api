using bingo_api.src.Entities.Scratch;

using bingo_api.src.DTOs.Response.Scratch.Jsonb;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchTicketResponseDto
{
   public Guid Id { get; set; }

    public decimal Value { get; set; }

    public ScratchAreaResponseDto Areas { get; set; }

    public Guid? ScratchPrizeId { get; set; }

    public Guid? ScratchBuyId { get; set; }

    public decimal? PrizeValue { get; set; }

    public static ScratchTicketResponseDto ConvertToDto(ScratchTicket entity)
    {
        return new ScratchTicketResponseDto
        {
            Id = entity.Id,
            Value = entity.Value,
            Areas = ScratchAreaResponseDto.ConvertToDto(entity.Areas),
            ScratchPrizeId = entity.ScratchPrizeId,
            ScratchBuyId = entity.ScratchBuyId,
            PrizeValue = entity.ScratchPrize?.Value
        };
    }
}