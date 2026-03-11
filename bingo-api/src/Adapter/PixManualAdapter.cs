using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class PixManualAdapter : IPaymentProvider
{
     public Task<PaymentGatewayResult> CreatePaymentAsync(
        Recharge recharge,
        Punter punter,
        PaymentMethod method,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new PaymentGatewayResult
        {
            GatewayTransactionId = recharge.Id.ToString(),
            Status = EPaymentStatus.WAITING_PAYMENT,
            QrCode = method.PixPayload,
            QrImageUrl = method.QrCodeUrl
        });
    }
}
