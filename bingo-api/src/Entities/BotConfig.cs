using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class BotConfig : Entity
{
    public bool Enabled  { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

   public BotConfig()
    {
        Enabled = false;
    }
    public BotConfig(Guid roomId)
    {
        Enabled = false;
        RoomId = roomId;
    }
}
