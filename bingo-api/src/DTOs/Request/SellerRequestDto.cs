using System.ComponentModel.DataAnnotations;

using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record SellerRequestDto : RegisterRequestDto
{

    [Required(ErrorMessage = "O campo Comissão é obrigatório.")]
    public decimal Comission { get; set; }
    internal static Seller ConvertToEntity(SellerRequestDto dto)
    {
        return new Seller(
            dto.Email,
            dto.Cpf,
            dto.DateBirth,
            dto.Comission
        );
    }
}
