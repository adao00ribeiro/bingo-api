using bingo_api.src.Context;
using bingo_api.src.Domain.Events;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces;


namespace bingo_api.src.Application.Handlers;

public class ScratchPrizeCreatedHandler : IDomainEventHandler<ScratchPrizeCreatedEvent>
{
    private readonly DataContext _context;

    public ScratchPrizeCreatedHandler(DataContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ScratchPrizeCreatedEvent domainEvent)
    {
        var prize = domainEvent.Prize;

        var ticket = _context.ScratchTickets
            .FirstOrDefault(t => t.Id == prize.ScratchTicketId);
        if (ticket == null) return;

        var punter = _context.Punters
            .FirstOrDefault(p => p.Id == ticket.ScratchBuy.PunterId);
        if (punter == null) return;

        _context.TransactionHistories.Add(new TransactionHistory
        {
            EntityType = "Punter", // Pode ser Seller se o participante for um Seller
            EntityId = punter.Id,
            PreviousBalance = punter.PrizeBalance, // Antes da alteração
            CurrentBalance = punter.PrizeBalance + prize.Value, // O saldo será alterado após o registro da transação
            Amount = prize.Value,
            Type = TransactionType.ScratchPrizeReceived, // Assume que Purchase é o tipo de transação para compra de cartela
        });
        punter.PrizeBalance += prize.Value;
        punter.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesWithoutEventsAsync(); // ✅ grava as alterações
    }
}
