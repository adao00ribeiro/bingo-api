using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchBuy : Entity
{
    public int Quantity { get; set; }
    public Guid SellerGameId { get; set; }
    public Guid PunterId { get; set; }
    public IEnumerable<ScratchTicket> ScratchTickets { get; set; }

    public ScratchBuy(int quantity, Guid sellerGameId, Guid punterId)
    {
        Quantity = quantity;
        SellerGameId = sellerGameId;
        PunterId = punterId;
    }
}
