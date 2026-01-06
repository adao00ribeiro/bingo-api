using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Request;

public class ScratchBuyRequestDto
{
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "ScratchGameId is required.")]
    public Guid ScratchSellerGameId { get; set; }

    [Required(ErrorMessage = "PunterId is required.")]
    public Guid PunterId { get; set; }


    internal static ScratchBuy ConvertToEntity(ScratchBuyRequestDto dto)
    {
        return new ScratchBuy(dto.Quantity, dto.ScratchSellerGameId, dto.PunterId);
    }

}
