using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using bingo_api.src.Entities;
using bingo_api.src.Factory;

namespace bingo_api.src.Structs;

public struct RoundMessage
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
    public Round Round { get; set; } = null;
    public IEnumerable<Prize> Prizes { get; set; }
    public IEnumerable<PrizeResult> Results { get; internal set; }

    public PrizeResult? CurrentPrizeResult { get; set; } = null;

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
        return JsonSerializer.Serialize(this, options);
    }
    // Atualiza detalhes de início da rodada
    public void UpdateMessageWithStartDetails(Accumulated bingoAccumulated, int drawnCount)
    {
        MaxNumbers = drawnCount;
        Accumulated = bingoAccumulated;
    }

    // Atualiza detalhes das bolas desenhadas
    public void UpdateBallDetails(int[] lastFourBalls, Accumulated bingoAccumulated, int drawnCount)
    {
        MainBall = lastFourBalls.ElementAtOrDefault(0);
        SecondBall = lastFourBalls.ElementAtOrDefault(1);
        ThirdBall = lastFourBalls.ElementAtOrDefault(2);
        ForthBall = lastFourBalls.ElementAtOrDefault(3);
        IsAccumulated = bingoAccumulated.Activated && (bingoAccumulated.MaximumNumberOfBalls >= drawnCount);
    }



}
