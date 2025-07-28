using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public record WithdrawalRequestDto
{
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Amount { get; set; }

    [Required]
    public Guid EntityId { get; set; } // Ou use o usuário autenticado
}
