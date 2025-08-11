using bingo_api.src.Enums;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Adapter;

public class CryptoAdapter : IPaymentProvider
{
    public Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method, string? network = null, string? Token = null , string? destinationAddress = null , string? txHash = null )
    {
        var recharge = new Recharge( value, ERechargeStatus.PENDING,punter.Id)
        {
                DestinationAddress = destinationAddress,
                TxHash = txHash,
                Network = network,
                Token = Token,
        };
        return Task.FromResult(recharge);
    }
}
