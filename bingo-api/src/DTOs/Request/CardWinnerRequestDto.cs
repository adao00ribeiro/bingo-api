using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record CardWinnerRequestDto
{
    [Required(ErrorMessage = "Value is required.")]
    public decimal Value { get; set; }

    [Required(ErrorMessage = "CardId is required.")]
    public Guid CardId { get; set; }

    [Required(ErrorMessage = "PrizeId is required.")]
    public Guid PrizeId { get; set; }

    internal static CardWinner ConvertToEntity(CardWinnerRequestDto dto)
    {
        return new CardWinner(dto.Value, dto.CardId, dto.PrizeId);
    }
}
