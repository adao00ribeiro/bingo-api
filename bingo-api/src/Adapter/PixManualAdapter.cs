using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class PixManualAdapter : IPaymentProvider
{
    public Task<Recharge> CreateRechargeAsync(decimal value, decimal amount, Punter punter, PaymentMethod method, string? network = null, string? Token = null, string? destinationAddress = null, string? txHash = null)
    {
        var recharge = new Recharge(value, amount, EPaymentStatus.PENDING, punter.Id)
        {
            Qrcode = method.QrCodeUrl,
            ImagemQrcode = method.Instructions
        };

        return Task.FromResult(recharge);
    }
}
