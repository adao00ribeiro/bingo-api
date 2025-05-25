using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces.Services;

public interface ITransactionParticipant
{
    decimal Balance { get; set; }
    decimal PrizeBalance { get; set; }
}
