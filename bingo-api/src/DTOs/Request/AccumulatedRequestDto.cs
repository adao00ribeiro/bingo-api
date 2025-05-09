
using System.ComponentModel.DataAnnotations;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public class AccumulatedRequestDto
{
    [Required(ErrorMessage = "{0} is required.")]
    public bool Activated { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public decimal MinimumValue { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public decimal MaximumValue { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public decimal CurrentValue { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public int MaximumNumberOfBalls { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public decimal CumulativePercentage { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public bool IncrementBallCumulative { get; set; }
    [Required(ErrorMessage = "{0} is required.")]
    public Guid RoomId { get; set; }

    public AccumulatedRequestDto(bool activated, decimal minimumValue, decimal maximumValue, decimal currentValue, int maximumNumberOfBalls, decimal cumulativePercentage, bool incrementBallCumulative, Guid roomId)
    {
        Activated = activated;
        MinimumValue = minimumValue;
        MaximumValue = maximumValue;
        CurrentValue = currentValue;
        MaximumNumberOfBalls = maximumNumberOfBalls;
        CumulativePercentage = cumulativePercentage;
        IncrementBallCumulative = incrementBallCumulative;
        RoomId = roomId;
    }
    internal static Accumulated ConvertToEntity(AccumulatedRequestDto dto)
    {
        return new Accumulated(
                dto.Activated,
        dto.MinimumValue,
        dto.MaximumValue,
        dto.CurrentValue,
         dto.MaximumNumberOfBalls,
         dto.CumulativePercentage,
         dto.IncrementBallCumulative,
                    dto.RoomId
        );
    }


}
