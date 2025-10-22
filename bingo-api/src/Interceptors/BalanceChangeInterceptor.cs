using System.Text.Json;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace bingo_api.src.Interceptors;

public class BalanceChangeInterceptor : SaveChangesInterceptor
{
    private readonly IWebSocketService _webSocketService;

    public BalanceChangeInterceptor(IWebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        // Pegamos todos os Punters modificados
        var entries = context.ChangeTracker.Entries<Punter>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var punterId = entry.Entity.Id;
            var originalBalance = entry.Property(p => p.Balance).OriginalValue;
            var currentBalance = entry.Property(p => p.Balance).CurrentValue;

            var originalPrize = entry.Property(p => p.PrizeBalance).OriginalValue;
            var currentPrize = entry.Property(p => p.PrizeBalance).CurrentValue;

            // Só envia se algum valor mudou
            if (originalBalance != currentBalance || originalPrize != currentPrize)
            {
                var message = new
                {
                    punterId,
                    balance = currentBalance,
                    prizeBalance = currentPrize
                };

                await _webSocketService.SendMessageToChannelAsync(
                    $"cash_box_{punterId}",
                    JsonSerializer.Serialize(message)
                );
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
