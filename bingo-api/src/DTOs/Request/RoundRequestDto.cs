
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public record RoundRequestDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor da cartela deve ser maior que zero.")]
    public decimal CardValue { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime StartedDate { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DefaultValue("4")]
    public int TimeBetweenBalls { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DefaultValue("90")]
    public int MaxBalls { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DefaultValue("3")]
    public int CardRows { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DefaultValue("5")]
    public int CardColumns { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DefaultValue("98522b7d-81d9-4c71-9ef4-fe505aae92b6")]
    public Guid RoomId { get; set; }
    public IEnumerable<PrizeRequestDto>? Prizes { get; set; }
    internal static Round ConvertToEntity(RoundRequestDto dto)
    {
        var round = new Round(
         dto.CardValue,
         dto.StartedDate,
         dto.TimeBetweenBalls,
         dto.MaxBalls,
         dto.CardRows,
         dto.CardColumns,
         dto.RoomId
        );

        if (dto.Prizes != null)
        {
            foreach (var prizeDto in dto.Prizes)
            {
                var prize = PrizeRequestDto.ConvertToEntity(prizeDto);
                round.AddPrize(prize);
            }
        }

        return round;
    }
}
