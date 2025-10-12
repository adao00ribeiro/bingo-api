using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class PunterWithdrawal: Withdrawal
{
    public Guid PunterId { get; set; }
    public Punter Punter { get; set; } = null!;
}
