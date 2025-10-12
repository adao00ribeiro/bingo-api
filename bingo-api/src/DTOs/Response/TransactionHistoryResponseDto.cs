using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;


namespace bingo_api.src.DTOs.Response;

public record TransactionHistoryResponseDto : EntityResponseDto
{
    public Guid EntityId { get; set; } // ID do Punter ou Seller
    public string EntityType { get; set; } // Nome da classe (ex: "Punter" ou "Seller")
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }

    public TransactionHistoryResponseDto(Guid id, DateTime CreatedAt, DateTime UpdatedAt, Guid entityId, string entityType, decimal previousBalance, decimal amount, decimal currentBalance, TransactionType type)
    : base(id, CreatedAt, UpdatedAt)
    {
        EntityId = entityId;
        EntityType = entityType;
        PreviousBalance = previousBalance;
        Amount = amount;
        CurrentBalance = currentBalance;
        CurrentBalance = previousBalance + amount;
        Type = type;
    }

    internal static TransactionHistoryResponseDto ConvertToDto(TransactionHistory r)
    {
        return new TransactionHistoryResponseDto(
            r.Id,
            r.CreatedAt,
            r.UpdatedAt,
            r.EntityId,
            r.EntityType,
            r.PreviousBalance,
            r.Amount,
            r.CurrentBalance,
            r.Type
            );
    }
}
