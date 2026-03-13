
namespace bingo_api.src.DTOs.Request.Scratch;

public record ScratchTicketRequestDto
{
    public decimal Value { get; set; }

    public Guid? ScratchPrizeId { get; set; }

    public Guid? ScratchBuyId { get; set; }
}
