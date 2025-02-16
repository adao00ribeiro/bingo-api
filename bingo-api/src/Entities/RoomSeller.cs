using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class RoomSeller : Entity
{

    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public Guid SellerId { get; set; }
    public  Seller Seller { get; set; }
    public string AssignedBy { get; set; }


    public RoomSeller(Guid roomId, Guid sellerId)
    {
        this.RoomId = roomId;
        this.SellerId = sellerId;
    }

}
