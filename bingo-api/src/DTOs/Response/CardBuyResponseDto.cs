using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public class CardBuyResponseDto
{
    public int Quantity { get; set; }
    public Guid RoundId { get; set; }
    public Guid PunterId { get; set; }

}
