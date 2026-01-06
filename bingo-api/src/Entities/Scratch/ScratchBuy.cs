using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchBuy : Entity
{
    public int Quantity { get; private set; }
    public Guid SellerGameId { get; private set; }
    public Guid PunterId { get; private set; }
    public IEnumerable<ScratchTicket> ScratchTickets { get;  private set; } 

    public ScratchBuy(int quantity, Guid sellerGameId, Guid punterId)
    {
        if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        Quantity = quantity;
        SellerGameId = sellerGameId;
        PunterId = punterId;
    }
}
