using System.ComponentModel.DataAnnotations.Schema;
using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class Room : Entity
{
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public OnlineHouse Owner { get; set; }
    public IEnumerable<RoomSeller>? RoomsSellers { get; set; }
    public IEnumerable<Round>? Rounds { get; set; }
    public Accumulated Accumulated { get; set; }
    public BotConfig BotConfig { get; set; }

    [NotMapped]
    public MediaAttachment? MediaAttachment { get; set; }
    public Room()
    {

    }
    public Room(string name, Guid ownerId)
    {
        this.Name = name;
        this.OwnerId = ownerId;
    }
}
