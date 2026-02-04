using bingo_api.src.Entities.Shared;
using bingo_api.src.Structs.OnlineHouse;


namespace bingo_api.src.Entities.Bingo;

public class OnlineHouse : Entity
{
    public string Name { get; set; }
    public string Hostname { get; set; }
    // public MediaAttachment? NavBarMediaAttachment { get; set; }
    // public MediaAttachment? LoginLogoMediaAttachment { get; set; }
    public Guid SellerId { get; set; }
    public Seller Seller { get; set; }
    public OnlineHouseSettings Settings { get; set; } = new();
    public IEnumerable<Punter> Punters { get; set; } = new List<Punter>();
    public IEnumerable<Room> OwnerRooms { get; set; } = new List<Room>();
    public IEnumerable<RoomSeller> ParticipantRooms { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public OnlineHouse(string name, Guid sellerId)
    {
        Name = name;
        SellerId = sellerId;
    }
}