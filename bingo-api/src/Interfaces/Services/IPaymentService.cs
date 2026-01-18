using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;

namespace bingo_api.src.Interfaces.Services;

public interface IPaymentService : IRepositoryBase<PaymentMethod>
{
    Task<Recharge> CreateRechargeAsync(decimal value, decimal amount, Punter punter, PaymentMethod method, string? network = null, string? Token = null, string? destinationAddress = null, string? txHash = null);
    Task SetActiveCurrentPayment(Guid guid);

}
