using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class BotConfig : Entity
{
    public bool Enabled { get; set; }
    public double PresenceRate { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    public BotConfig()
    {
        Enabled = false;
    }
    public BotConfig(bool enabled, double presenceRate, Guid roomId)
    {
        Enabled = enabled;
        PresenceRate = presenceRate;
        RoomId = roomId;
    }
    public BotConfig(Guid roomId)
    {
        Enabled = false;
        RoomId = roomId;
    }
}
