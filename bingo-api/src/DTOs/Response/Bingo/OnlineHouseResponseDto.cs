using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities.Bingo;
using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response.Bingo;

public record OnlineHouseResponseDto : EntityResponseDto
{
    public string Name { get; set; }
    public string Hostname { get; set; } 
    public Guid SellerId { get; set; }
    public SellerResponseDto Seller { get; set; } 
    public OnlineHouseSettings Settings { get; set; }
    public IEnumerable<PunterResponseDto> Punters { get; set; }
    public IEnumerable<RoomResponseDto> OwnerRooms { get; set; }
    public IEnumerable<RoomSellerResponseDto> ParticipantRooms { get; set; }

    protected OnlineHouseResponseDto(Guid id, string name, string hostname, OnlineHouseSettings settings, DateTime CreatedAt,
        DateTime UpdatedAt)
    : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Name = name;
        Hostname = hostname;
        Settings = settings;
    }

    internal static OnlineHouseResponseDto ConvertToDto(OnlineHouse onlineHouse)
    {
        return new OnlineHouseResponseDto(
            onlineHouse.Id,
            onlineHouse.Name,
            onlineHouse.Hostname,
            onlineHouse.Settings,
            onlineHouse.CreatedAt,
            onlineHouse.UpdatedAt
        );
    }
}
