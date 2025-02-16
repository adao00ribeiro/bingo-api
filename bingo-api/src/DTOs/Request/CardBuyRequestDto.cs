using System.ComponentModel.DataAnnotations;

namespace bingo_api.src.DTOs.Request;

public class CardBuyRequestDto
{
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "RoundId is required.")]
    public Guid RoundId { get; set; }

    [Required(ErrorMessage = "PunterId is required.")]
    public Guid PunterId { get; set; }

}
