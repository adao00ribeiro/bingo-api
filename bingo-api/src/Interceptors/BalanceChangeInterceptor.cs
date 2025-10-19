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
        if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries<Punter>()
            .Where(e => e.State == EntityState.Modified &&
                        e.Property(p => p.Balance).IsModified);

        foreach (var entry in entries)
        {
            var oldBalance = entry.Property(p => p.Balance).OriginalValue;
            var newBalance = entry.Property(p => p.Balance).CurrentValue;

            if (oldBalance != newBalance)
            {
                var punterId = entry.Entity.Id;
                var message = new
                {
                    punterId = punterId,
                    balance = newBalance
                };

                await _webSocketService.SendMessageToChannel($"cash_box_{punterId}", JsonSerializer.Serialize(message));
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
