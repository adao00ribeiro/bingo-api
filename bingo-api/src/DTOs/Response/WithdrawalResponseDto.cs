using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response;

public record WithdrawalResponseDto : EntityResponseDto
{
    public string WithdrawalType { get; set; }
    public decimal Amount { get; set; }
    public EPaymentStatus Status { get; set; }
    public DateTime? ConfirmedAt { get; set; } = null;



    public WithdrawalResponseDto(Guid id, string withdrawalType, decimal amount, EPaymentStatus status, DateTime? confirmedAt, DateTime createdAt, DateTime updatedAt) : base(id, createdAt, updatedAt)
    {
        WithdrawalType = withdrawalType;
        Amount = amount;
        Status = status;
        ConfirmedAt = confirmedAt;
    }
    public static WithdrawalResponseDto ConvertToDto(Withdrawal entity)
    {
        return entity switch
        {
            SellerWithdrawal sw => SellerWithdrawalResponseDto.ConvertToDto(sw),
            PunterWithdrawal pw => PunterWithdrawalResponseDto.ConvertToDto(pw),
            _ => new WithdrawalResponseDto(entity.Id, entity.WithdrawalType, entity.Amount, entity.Status, entity.ConfirmedAt, entity.CreatedAt, entity.UpdatedAt)
        };
    }
}