using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces;

namespace bingo_api.src.Domain.Events;

public class ScratchPrizeCreatedEvent: IDomainEvent
{
       public ScratchPrize Prize { get; }
       public ScratchPrizeCreatedEvent(ScratchPrize prize)
    {
        Prize = prize;
    }
}
