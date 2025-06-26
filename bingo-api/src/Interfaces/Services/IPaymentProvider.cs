using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities;

namespace bingo_api.src.Interfaces.Services;

public interface IPaymentProvider
{
    Task<Recharge> CreateRechargeAsync(decimal value, Punter punter, PaymentMethod method);
}