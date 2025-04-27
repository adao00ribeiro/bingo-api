using System.Text.Json;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;

namespace bingo_api.src.Structs;

public class RoundMessage
{
    public Guid Id { get; set; }
    public bool Finished { get; set; } = false;
    public bool Started { get; set; } = true;
    public int MainBall { get; set; } = 0;
    public int SecondBall { get; set; } = 0;
    public int ThirdBall { get; set; } = 0;
    public int ForthBall { get; set; } = 0;
    public int MaxNumbers { get; set; } = 0;
    public List<int> Numbers { get; set; } = new List<int>();
    public Accumulated Accumulated { get; set; } = null;
    public bool IsAccumulated { get; set; } = false;
    public RoundResponseDto Round { get; set; } = null;
    public IEnumerable<PrizeResult> Results { get;  set; }
    public PrizeResult? CurrentPrizeResult { get; set; } = null;
    public RoundMessage()
    {

    }
    public RoundMessage(Guid roundId)
    {
        this.Id = roundId;
    }

    public string JsonSerializerRound()
    {

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        var text = JsonSerializer.Serialize(this, options);
        //File.WriteAllText("Socketjson.json", text);
        return text;
    }
    public RoundMessage Clone()
{
    return new RoundMessage
    {
        Id = this.Id,
        Finished = this.Finished,
        Started = this.Started,
        MainBall = this.MainBall,
        SecondBall = this.SecondBall,
        ThirdBall = this.ThirdBall,
        ForthBall = this.ForthBall,
        MaxNumbers = this.MaxNumbers,
        Numbers = new List<int>(this.Numbers),
        Accumulated = this.Accumulated,
        IsAccumulated = this.IsAccumulated,
        Round = this.Round ,
        Results = this.Results,
        CurrentPrizeResult = this.CurrentPrizeResult
    };
}

}
