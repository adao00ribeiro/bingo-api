using System.Text.Json.Serialization;

namespace bingo_api.src.Structs;

public class TimelineEvent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RoundMessage eventData { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Delay { get; set; }

}
