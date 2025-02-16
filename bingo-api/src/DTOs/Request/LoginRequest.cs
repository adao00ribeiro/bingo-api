using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public record LoginRequest
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
    [DefaultValue("jogo@gmail.com")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DefaultValue("Jodo123+")]
    public string Password { get; set; }
}
