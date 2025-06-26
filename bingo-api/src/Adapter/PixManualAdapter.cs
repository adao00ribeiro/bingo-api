using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class PixManualAdapter : IPaymentProvider
{
    public Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method)
    {
        var recharge = new Recharge(value, ERechargeStatus.PENDING, punter.Id)
        {
            Qrcode = method.QrCodeUrl,
            ImagemQrcode = method.Instructions
        };

        return Task.FromResult(recharge);
    }
}
