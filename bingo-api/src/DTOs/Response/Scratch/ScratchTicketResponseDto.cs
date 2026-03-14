using bingo_api.src.Entities.Scratch;

using bingo_api.src.DTOs.Response.Scratch.Jsonb;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchTicketResponseDto
{
   public Guid Id { get; set; }

    public decimal Value { get; set; }

    public IEnumerable<ScratchAreaResponseDto> Areas { get; set; }

    public Guid? ScratchPrizeId { get; set; }

    public Guid? ScratchBuyId { get; set; }

    public static ScratchTicketResponseDto ConvertToDto(ScratchTicket entity)
    {
          var areas = entity.Attributes.Areas?.Select(r => ScratchAreaResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<ScratchAreaResponseDto>();

        return new ScratchTicketResponseDto
        {
            Id = entity.Id,
            Value = entity.Value,
            Areas = areas,
            ScratchBuyId = entity.ScratchBuyId,
        };
    }
}