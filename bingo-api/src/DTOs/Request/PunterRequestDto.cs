
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record PunterRequestDto : RegisterRequestDto
{
    [Required(ErrorMessage = "O campo Id do dono da sala é Obrigatorio")]
    public Guid SellerId { get; set; }
    public string Name { get; set; } = "";

    [DefaultValue("")]
    public string RegisteredWithTag { get; set; }= "";
    internal static Punter ConvertToEntity(PunterRequestDto dto)
    {
        return new Punter(
             dto.Email,
             dto.Name,
             dto.Cpf,
             dto.DateBirth,
             dto.SellerId,
             dto.RegisteredWithTag
        );
    }
}
