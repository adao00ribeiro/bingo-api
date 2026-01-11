using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class RoomSeller : Entity
{

    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public Guid OnlineHouseId { get; set; }
    public OnlineHouse OnlineHouse { get; set; }
    public string AssignedBy { get; set; }


    public RoomSeller(Guid roomId, Guid onlineHouseId)
    {
        this.RoomId = roomId;
        this.OnlineHouseId = onlineHouseId;
    }

}
