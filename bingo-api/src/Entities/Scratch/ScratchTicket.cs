using bingo_api.src.Entities.Shared;
using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.Entities.Scratch;

public class ScratchTicket : Entity
{
    public decimal Value { get; set; } 
    public ScratchArea Areas { get; set; } = new();
    public Guid? ScratchPrizeId { get; set; }
    public ScratchPrize ScratchPrize { get; set; }
    public Guid? ScratchBuyId { get; set; }
    public ScratchBuy ScratchBuy { get; set; }
}
