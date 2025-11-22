using bingo_api.src.Enums;

namespace bingo_api.src.Entities.Shared;

public abstract class Withdrawal : Entity
{
    public decimal Amount { get; set; }
    public EWithdrawalStatus Status { get; set; }

}
