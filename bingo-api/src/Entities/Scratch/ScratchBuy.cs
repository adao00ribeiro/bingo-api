using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchBuy : Entity
{
    public decimal Value { get; set; }
    public int Quantity { get; private set; }
    public ScratchGameOverride ScratchGameOverride { get; set; }
    public Guid ScratchGameOverrideId { get; private set; }
    public Punter Punter { get; set; }
    public Guid PunterId { get; private set; }
    public IEnumerable<ScratchTicket> ScratchTickets { get; private set; }

    public ScratchBuy(int quantity, Guid scratchGameOverrideId, Guid punterId)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        Quantity = quantity;
        ScratchGameOverrideId = ScratchGameOverrideId;
        PunterId = punterId;
    }
}
