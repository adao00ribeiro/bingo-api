using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services.Transactions;

public class DepositTransaction : ITransactionStrategy
{
    public void Execute(ITransactionParticipant participant, decimal amount)
    {
         participant.Balance += amount;
    }
}
