using System.ComponentModel.DataAnnotations.Schema;
using bingo_api.src.Enums;

namespace bingo_api.src.Entities.Shared;

public abstract class Withdrawal : Entity
{
    public decimal Amount { get; set; }
    public EPaymentStatus Status { get; set; }
    public DateTime? ConfirmedAt { get; set; } = null;
    [NotMapped]
    public string WithdrawalType =>
    GetType().Name.Replace("Withdrawal", "");
}
