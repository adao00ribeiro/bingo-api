using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Repositories.Scratch;

public interface IScratchTicketRepository : IRepositoryBase<ScratchTicket>
{
     Task<ScratchTicket?> BuyTicket(Guid PunterId);
     Task FinishScratchAsync(Guid ticketId, Guid userId);
}
