using bingo_api.src.Adapter;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class PaymentService : IPaymentService
{
    private readonly Dictionary<EPaymentMethodType, IPaymentProvider> _providers;

    public PaymentService(IEnumerable<IPaymentProvider> providers)
    {
        _providers = providers.ToDictionary(
            p => p switch
            {
                PixManualAdapter => EPaymentMethodType.PIXMANUAL,
                PushPayAdapter => EPaymentMethodType.PUSHPAY,
                _ => throw new Exception("Adapter não registrado")
            });
    }
    public async Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method)
    {
        if (!_providers.TryGetValue(method.Type, out var provider))
            throw new Exception("Tipo de pagamento não suportado");

        return await provider.CreateRechargeAsync(value, punter, method);
    }
}
