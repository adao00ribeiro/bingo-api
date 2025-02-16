using System.Text.Json.Serialization;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class Accumulated : Entity
{
    public bool Activated { get; set; }
    public decimal MinimumValue { get; set; }
    public decimal MaximumValue { get; set; }
    public decimal CurrentValue { get; set; }
    public int MaximumNumberOfBalls { get; set; }
    public decimal CumulativePercentage { get; set; }
    public bool IncrementBallCumulative { get; set; }
    public Guid RoomId { get; set; }

    [JsonIgnore]
    public  Room Room { get; set; }

    public Accumulated()
    {
        this.Activated = false;
        this.MinimumValue = 10;
        this.MaximumValue = 1000;
        this.CurrentValue = 10;
        this.MaximumNumberOfBalls = 40;
        this.CumulativePercentage = 1;
        this.IncrementBallCumulative = false;
    }
}
