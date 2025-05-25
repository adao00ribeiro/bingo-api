using System.Text.Json.Serialization;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record BotConfigResponseDto
{
    public Guid Id { get; set; }
    public bool Enabled { get; set; }
    public double PresenceRate { get; set; }
    public Guid RoomId { get; set; }
    [JsonIgnore]
    public RoomResponseDto? Room { get; set; }
    public BotConfigResponseDto(Guid id, bool enabled, double presenceRate, Guid roomId, RoomResponseDto? room)
    {
        Id = id;
        Enabled = enabled;
        PresenceRate = presenceRate;
        RoomId = roomId;
        Room = room;
    }
    internal static BotConfigResponseDto ConvertToDto(BotConfig config)
    {
        var roomResponse = config.Room != null ? RoomResponseDto.ConvertToDto(config.Room) : null;
        return new BotConfigResponseDto(
                config.Id,
                config.Enabled,
                config.PresenceRate,
                config.RoomId,
                roomResponse
        );
    }
}
