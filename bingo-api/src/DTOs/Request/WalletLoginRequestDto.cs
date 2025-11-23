using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public record WalletLoginRequestDto
{
    [Required(ErrorMessage = "{0} is required.")]
    public string Address { get; set; }
    [Required(ErrorMessage = "{0} is required.")]

    public string Message { get; set; }
    [Required(ErrorMessage = "{0} is required.")]

    public string Signature { get; set; }
}
