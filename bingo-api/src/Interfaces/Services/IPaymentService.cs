using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IPaymentService
{
    Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method);
}
