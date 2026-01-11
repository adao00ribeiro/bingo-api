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

    

    protected OnlineHouseResponseDto(Guid id, string name, string hostname, OnlineHouseSettings settings, Guid sellerId, DateTime CreatedAt,
        DateTime UpdatedAt, IEnumerable<RoomResponseDto> ownerRooms, IEnumerable<RoomSellerResponseDto> participantRooms)
    : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Name = name;
        Hostname = hostname;
        Settings = settings;
        SellerId = sellerId;
        OwnerRooms = ownerRooms;
        ParticipantRooms = participantRooms;
    }

    internal static OnlineHouseResponseDto ConvertToDto(OnlineHouse onlineHouse)
    {
        var ownerRoomsReponse = onlineHouse.OwnerRooms?.Select(r => RoomResponseDto.ConvertToDtoToOnlineHouse(r)) ?? Enumerable.Empty<RoomResponseDto>();
        var participantRoomsReponse = onlineHouse.ParticipantRooms?.Select(r => RoomSellerResponseDto.ConvertToDtoToOnlineHouse(r)) ?? Enumerable.Empty<RoomSellerResponseDto>();

        return new OnlineHouseResponseDto(
            onlineHouse.Id,
            onlineHouse.Name,
            onlineHouse.Hostname,
            onlineHouse.Settings,
            onlineHouse.SellerId,
            onlineHouse.CreatedAt,
            onlineHouse.UpdatedAt,
            ownerRoomsReponse,
            participantRoomsReponse
        );
    }
}
