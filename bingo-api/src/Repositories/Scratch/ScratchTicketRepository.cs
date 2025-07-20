using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Interfaces.Repositories.Scratch;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Scratch;

public class ScratchTicketRepository : RepositoryBase<ScratchTicket>, IScratchTicketRepository
{
    public ScratchTicketRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<ScratchTicket?> BuyTicket(Guid punterId)
    {
        var punter = await Context.Punters.FindAsync(punterId);

        if (punter is null)
        {
            throw new Exception("Usuário não encontrado");
        }

        using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            var ticket = await Context.ScratchTickets
     .FromSqlRaw(@"
        SELECT * FROM ""scratch_tickets""
        WHERE (""attributes""->>'PunterId') IS NULL
        OR (""attributes""->>'PunterId') = '00000000-0000-0000-0000-000000000000'
        ORDER BY ""updated_at""
        FOR UPDATE SKIP LOCKED
        LIMIT 1
    ")
     .Include(x => x.ScratchGame)
     .FirstOrDefaultAsync();

            if (ticket is null)
            {
                await transaction.RollbackAsync();
                return null;
            }
            if (punter.Balance < ticket.ScratchGame.Price)
            {
                throw new Exception("Saldo Insuficiente");
            }
            // Marca o ticket como usado
            ticket.Attributes.PunterId = punterId;
            // Atualiza saldo do jogador
            var previousBalance = punter.Balance;
            punter.Balance -= ticket.ScratchGame.Price;

            var transactionHistory = new TransactionHistory
            {
                EntityType = "Punter",
                EntityId = punterId,
                PreviousBalance = previousBalance,
                CurrentBalance = punter.Balance,
                Amount = ticket.ScratchGame.Price,
                Type = TransactionType.ScratchPurchased,
                CreateAt = DateTime.UtcNow
            };

            Context.TransactionHistories.Add(transactionHistory);
            Context.ScratchTickets.Update(ticket);
            await Context.SaveChangesAsync();

            await transaction.CommitAsync();

            return ticket;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task FinishScratchAsync(Guid ticketId, Guid punterId)
    {
        var punter = await Context.Punters.FindAsync(punterId);
        if (punter is null)
            throw new Exception("Usuário não encontrado");

        var ticket = await Context.ScratchTickets
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.Attributes.PunterId == punterId);

        if (ticket is null)
            throw new Exception("Ticket não encontrado ou não pertence ao usuário");

        if (ticket.Revealed)
            throw new Exception("Ticket já foi revelado");

        ticket.Revealed = true;
        ticket.UpdateAt = DateTime.UtcNow;

        // Conta os símbolos vencedores
        var grouped = ticket.Attributes.Items
            .GroupBy(a => a.Symbol)
            .Select(g => new { Symbol = g.Key, Count = g.Count(), IsWinner = g.All(x => x.IsWinner) })
            .Where(g => g.Count >= 3 && g.IsWinner)
            .FirstOrDefault();

        if (grouped is not null)
        {
            // Obtém o valor do símbolo premiado no jogo
            var game = await Context.ScratchGames.FirstOrDefaultAsync(g => g.Id == ticket.ScratchGameId);
            var matchedSymbol = game?.Attributes?.Symbols.FirstOrDefault(s => s.Symbol == grouped.Symbol);

            if (matchedSymbol is not null)
            {
                var prize = matchedSymbol.PrizeValue * ticket.Multiplier;
                ticket.PrizeWon = prize;

                // Atualiza saldo do jogador
                var previousBalance = punter.Balance;
                punter.Balance += prize;

                var transaction = new TransactionHistory
                {
                    EntityType = "Punter",
                    EntityId = punter.Id,
                    PreviousBalance = previousBalance,
                    CurrentBalance = punter.Balance,
                    Amount = prize,
                    Type = TransactionType.Reward,
                    CreateAt = DateTime.UtcNow
                };

                Context.TransactionHistories.Add(transaction);
            }
        }
        else
        {
            ticket.PrizeWon = 0;
        }
        // Libera o ticket (se for reaproveitado)
        ticket.Attributes.PunterId = Guid.Empty;

        Context.ScratchTickets.Update(ticket);
        Context.Punters.Update(punter);
        await Context.SaveChangesAsync();
    }

}
