using bingo_api.src.Domain.Events;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchPrize : Entity
{
    public decimal Value  { get; set; }
    public Guid ScratchTicketId { get; set; }
    public ScratchTicket ScratchTicket { get; set; }

    public ScratchPrize(decimal value,  Guid scratchTicketId)
    {
        Value = value;
        ScratchTicketId = scratchTicketId;

        AddDomainEvent(new ScratchPrizeCreatedEvent(this));
    }

}
