using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;
using bingo_api.src.Enums;



namespace bingo_api.src.DTOs.Request;

public record PrizeRequestDto
{
    [Required(ErrorMessage = "Value is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than zero.")]
    public decimal Value { get; set; }

    [Required(ErrorMessage = "PrizeType is required.")]
    public EPrizeType Type { get; set; }

    [Required(ErrorMessage = "Round is required.")]

    public Guid RoundId { get; set; }

    internal static Prize ConvertToEntity(PrizeRequestDto dto)
    {
        Prize prize = new Prize(dto.Value, dto.Type, dto.RoundId);
        return prize;
    }
}