using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IPaymentService
{
    Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method, string? network = null, string? Token = null, string? destinationAddress = null, string? txHash = null);
}
