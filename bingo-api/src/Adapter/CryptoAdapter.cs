using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class CryptoAdapter : IPaymentProvider
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
            Status = Enums.EPaymentStatus.WAITING_PAYMENT,
           // WalletAddress = method.WalletAddress,
           // Network = method.Network
        });
    }
}
