

using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class TransactionHistory : Entity
{
    public Guid EntityId { get; set; } // ID do Punter ou Seller
    public string EntityType { get; set; } // Nome da classe (ex: "Punter" ou "Seller")
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }

    public TransactionHistory() { }

    public TransactionHistory(Guid entityId, string entityType, decimal previousBalance,decimal currentBalance, decimal amount, TransactionType type)
    {
        EntityId = entityId;
        EntityType = entityType;
        PreviousBalance = previousBalance;
        Amount = amount;
        CurrentBalance = currentBalance;
        Type = type;
    }
}
