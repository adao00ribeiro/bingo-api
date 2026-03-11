using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchBuy : Entity
{
    public decimal Value { get; set; }
    public int Quantity { get; private set; }
    public Guid ScratchSellerGameId { get; private set; }
    public Guid PunterId { get; private set; }
    public Punter Punter { get; set; }
    public ScratchSellerGame ScratchSellerGame { get; set; }
    public IEnumerable<ScratchTicket> ScratchTickets { get; private set; }

    public ScratchBuy(int quantity, Guid scratchSellerGameId, Guid punterId)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        Quantity = quantity;
        ScratchSellerGameId = scratchSellerGameId;
        PunterId = punterId;
    }
}
