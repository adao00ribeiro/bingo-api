
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record RoomSellerRequestDto
{
    [Required(ErrorMessage = "O ID da sala é obrigatório.")]
    public Guid RoomId { get; set; }

    [Required(ErrorMessage = "O ID do vendedor é obrigatório.")]
    public Guid SellerId { get; set; }




    internal static RoomSeller ConvertToEntity(RoomSellerRequestDto dto)
    {
        return new RoomSeller(dto.RoomId, dto.SellerId);
    }
}
