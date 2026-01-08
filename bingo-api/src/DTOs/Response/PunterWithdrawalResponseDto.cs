using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response;

public record PunterWithdrawalResponseDto : WithdrawalResponseDto
{
    public Guid PunterId { get; set; }
    public PunterResponseDto Punter { get; set; }

    public PunterWithdrawalResponseDto(Guid id, string withdrawalType, decimal amount, EPaymentStatus status, DateTime? confirmedAt, DateTime createdAt, DateTime updatedAt, Guid punterId, PunterResponseDto punter)
                                : base(id, withdrawalType, amount, status, confirmedAt, createdAt, updatedAt)
    {
        PunterId = punterId;
        Punter = punter;
    }
    public static PunterWithdrawalResponseDto ConvertToDto(PunterWithdrawal w)
    {
        return new PunterWithdrawalResponseDto(w.Id, w.WithdrawalType, w.Amount, w.Status, w.ConfirmedAt, w.CreatedAt, w.UpdatedAt, w.PunterId, PunterResponseDto.ConvertToDto(w.Punter));
    }
}
