using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Request;

public record RechargeRequestDto
{
    public Guid Id { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Value { get; set; }
    public decimal Amount { get; set; }
    [Required(ErrorMessage = "O ID do apostador é obrigatório.")]
    public Guid PunterId { get; set; }
    public string Qrcode { get; set; }
    public string ImagemQrcode { get; set; }
    internal static Recharge ConvertToEntity(RechargeRequestDto dto)
    {
        return new Recharge(dto.Value, dto.Amount, ERechargeStatus.PENDING, dto.PunterId);
    }
}
