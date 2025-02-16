using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class Room : Entity
{
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public  Seller Owner { get; set; }
    public  IEnumerable<RoomSeller>? RoomsSellers { get; set; }
    public  IEnumerable<Round>? Rounds { get; set; }
    public  Accumulated Accumulated { get; set; }
    public Room(string name, Guid ownerId)
    {
        this.Name = name;
        this.OwnerId = ownerId;
    }
}
