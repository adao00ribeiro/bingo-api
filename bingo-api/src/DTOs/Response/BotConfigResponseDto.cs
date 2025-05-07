using System.Text.Json.Serialization;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record BotConfigResponseDto
{
    public Guid Id { get;  set; } 
    public bool Enabled { get; set; }
    public Guid RoomId { get; set; }

    
    [JsonIgnore]
    public RoomResponseDto? Room { get; set; }
    public BotConfigResponseDto(Guid id ,bool enabled, Guid roomId, RoomResponseDto? room)
    {
        Id = id;
        Enabled = enabled;
        RoomId = roomId;
        Room = room;
    }
    internal static BotConfigResponseDto ConvertToDto(BotConfig config)
    {
        var roomResponse = config.Room != null ? RoomResponseDto.ConvertToDto(config.Room) : null;
        return new BotConfigResponseDto(
                config.Id,
                config.Enabled,
                config.RoomId,
                roomResponse
        );
    }
}
