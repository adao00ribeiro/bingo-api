using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public record DepositRequestDto
{
    [Required(ErrorMessage = "Value is required.")]
    public decimal Value { get; set; }
    public string? Network { get; set; }
    public string? Token { get; set; } 
    public string? transactionHash { get; set; } 
    public string? address { get; set; } 

}
