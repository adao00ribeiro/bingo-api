using bingo_api.src.Context;
using bingo_api.src.Domain.Events;
using bingo_api.src.Interfaces;
using bingo_api.src.Services;

namespace bingo_api.src.Application.Handlers;

public class PixPayloadUpdatedHandler : IDomainEventHandler<PixPayloadUpdatedEvent>
{
    private readonly DataContext _context;

    public PixPayloadUpdatedHandler(DataContext context)
    {
        _context = context;
    }
    public async Task HandleAsync(PixPayloadUpdatedEvent domainEvent)
    {
        var paymentMethod = domainEvent.PaymentMethod;

        if (string.IsNullOrEmpty(paymentMethod.PixPayload))
            return;

        var qrCode = PixQrCodeService.GerarQrCodeBase64(paymentMethod.PixPayload);
        paymentMethod.QrCodeUrl = qrCode;
        paymentMethod.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesWithoutEventsAsync(); // ✅ grava as alterações
    }
}
