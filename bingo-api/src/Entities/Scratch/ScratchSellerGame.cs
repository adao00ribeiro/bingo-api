using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchSellerGame : Entity
{
    public Guid SellerId { get; set; }
    public Guid ScratchGameId { get; set; }
    public Seller Seller { get; set; }
    public ScratchGame ScratchGame { get; set; }
    public ICollection<ScratchTicket> ScratchTickets { get; set; }
    public ScratchSellerGame(Guid sellerId, Guid scratchGameId)
    {
        SellerId = sellerId;
        ScratchGameId = scratchGameId;
    }
}
