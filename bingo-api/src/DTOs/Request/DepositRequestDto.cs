using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public record DepositRequestDto
{
    [Required(ErrorMessage = "Value is required.")]
    public decimal Value { get; set; }
}
