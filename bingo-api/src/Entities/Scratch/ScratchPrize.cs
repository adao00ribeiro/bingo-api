using bingo_api.src.Domain.Events;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces;

namespace bingo_api.src.Entities.Scratch;

public class ScratchPrize : Entity
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid ScratchGameId { get; set; }
    public Guid ScratchTicketId { get; set; }
    public ScratchGame ScratchGame { get; set; }
    public ScratchTicket ScratchTicket { get; set; }

    public ScratchPrize(string description, decimal amount, Guid scratchGameId, Guid scratchTicketId)
    {
        Description = description;
        Amount = amount;
        ScratchGameId = scratchGameId;
        ScratchTicketId = scratchTicketId;

        AddDomainEvent(new ScratchPrizeCreatedEvent(this));
    }

}
