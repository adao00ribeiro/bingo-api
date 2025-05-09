using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public class AccumulatedResponseDto
{

    public Guid Id { get; set; }
    public bool Activated { get; set; }
    public decimal MinimumValue { get; set; }
    public decimal MaximumValue { get; set; }
    public decimal CurrentValue { get; set; }
    public int MaximumNumberOfBalls { get; set; }
    public decimal CumulativePercentage { get; set; }
    public bool IncrementBallCumulative { get; set; }
    public Guid RoomId { get; set; }

    public AccumulatedResponseDto(Guid id, bool activated, decimal minimumValue, decimal maximumValue, decimal currentValue, int maximumNumberOfBalls, decimal cumulativePercentage, bool incrementBallCumulative, Guid roomId)
    {
        Id = id;
        Activated = activated;
        MinimumValue = minimumValue;
        MaximumValue = maximumValue;
        CurrentValue = currentValue;
        MaximumNumberOfBalls = maximumNumberOfBalls;
        CumulativePercentage = cumulativePercentage;
        IncrementBallCumulative = incrementBallCumulative;
        RoomId = roomId;
    }
    internal static object? ConvertToDto(Accumulated accumulated)
    {
        return new AccumulatedResponseDto(
            accumulated.Id,
            accumulated.Activated,
       accumulated.MinimumValue,
       accumulated.MaximumValue,
       accumulated.CurrentValue,
       accumulated.MaximumNumberOfBalls,
       accumulated.CumulativePercentage,
       accumulated.IncrementBallCumulative,
       accumulated.RoomId
        );
    }
}
