using bingo_api.src.Adapter;
using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IPaymentProvider
{
      Task<PaymentGatewayResult> CreatePaymentAsync(
        Recharge recharge,
        Punter punter,
        PaymentMethod method,
        CancellationToken cancellationToken);
    
}