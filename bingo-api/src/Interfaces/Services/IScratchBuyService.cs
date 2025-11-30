using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.Interfaces.Services;

public interface IScratchBuyService
{
    Task<ScratchTicket> Buy(Guid punterId, ScratchBuy buy);

    Task<ScratchTicket?> RevealTicket(Guid ticketId);
}
