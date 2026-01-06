using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public class CardBuyRequestDto
{
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "RoundId is required.")]
    public Guid RoundId { get; set; }

    [Required(ErrorMessage = "PunterId is required.")]
    public Guid PunterId { get; set; }


    internal static CardBuy ConvertToEntity(CardBuyRequestDto dto)
    {
        return new CardBuy(dto.Quantity, dto.RoundId, dto.PunterId);
    }

}
