using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Exceptions;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Services.Scratchcard;

public class ScratchBuyService
    (
    DataContext dataContext,
    IScratchBuyRepository scratchBuyRepository
    ) : IScratchBuyService
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IScratchBuyRepository _scratchBuyRepository = scratchBuyRepository;

    public Task<ScratchTicket> Buy(Guid punterId, ScratchBuy buy)
    {
        throw new NotImplementedException();
    }

    public async Task<ScratchBuy> CreateAsync(int quantity, Guid scratchGameOverrideId, Guid punterId)
    {

        // Carrega terminal e modality override
        var punter = await _dataContext.Punters
                                .FirstOrDefaultAsync(t => t.Id == punterId);
        if (punter == null) throw new Exception("Apostador not found");

        var gameOverride = await _dataContext.ScratchGameOverrides
                                .FirstOrDefaultAsync(m => m.Id == scratchGameOverrideId);
        if (gameOverride == null) throw new Exception("Modality override not found");

        // Calcula valor total
        var totalValue = gameOverride.CardValue * quantity;

        decimal totalBalance = punter.Balance + punter.PrizeBalance;

        if (totalBalance < totalValue)
            throw new Exception("Saldo insuficiente. Por favor, recarregue sua conta.");
      
         ScratchBuy buy = ScratchBuy.Create(gameOverride, punter, quantity);
         await _scratchBuyRepository.AddAsync(buy);
         return buy;
    }

    public Task<ScratchTicket?> RevealTicket(Guid ticketId)
    {
        throw new NotImplementedException();
    }
}