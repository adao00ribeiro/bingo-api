using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchPrize :Entity
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid ScratchGameId { get; set; }
    public Guid ScratchTicketId { get; set; }
    public ScratchGame ScratchGame { get; set; }
    public ScratchTicket ScratchTicket { get; set; }
}
