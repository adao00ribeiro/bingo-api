using bingo_api.src.Adapter;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Services;

public class PaymentService : RepositoryBase<PaymentMethod>, IPaymentService
{
    private readonly Dictionary<EPaymentMethodType, IPaymentProvider> _providers;

    public PaymentService(DataContext dataContext, IEnumerable<IPaymentProvider> providers) : base(dataContext)
    {
        _providers = providers.ToDictionary(
            p => p switch
            {
                PixManualAdapter => EPaymentMethodType.PIXMANUAL,
                PushPayAdapter => EPaymentMethodType.PUSHPAY,
                CryptoAdapter => EPaymentMethodType.CRYPTO,
                _ => throw new Exception("Adapter não registrado")
            });
    }
    public async Task<Recharge> CreateRechargeAsync(decimal value, decimal amount, Punter punter, PaymentMethod method, string? network = null, string? Token = null, string? destinationAddress = null, string? txHash = null)
    {
        if (!_providers.TryGetValue(method.Type, out var provider))
            throw new Exception("Tipo de pagamento não suportado");

        return await provider.CreateRechargeAsync(value, amount, punter, method, network, Token, destinationAddress, txHash);
    }

    public async Task SetActiveCurrentPayment(Guid sellerId)
    {
       await Context.PaymentMethods
    .Where(x =>
        x.Type != EPaymentMethodType.CRYPTO &&
        Context.OnlineHouses
            .Where(h => h.SellerId == sellerId)
            .Select(h => h.Id)
            .Contains(x.OnlineHouseId)
    )
    .ExecuteUpdateAsync(setters =>
        setters.SetProperty(p => p.Active, false)
    );
    }

}
