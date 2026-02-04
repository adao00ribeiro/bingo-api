
using bingo_api.src.DTOs.Response.Bingo;
using bingo_api.src.Entities;


namespace bingo_api.src.DTOs.Response;

public record RoomResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public AccumulatedResponseDto? Accumulated { get; set; }
    public OnlineHouseResponseDto? Owner { get; set; }
    public IEnumerable<RoomSellerResponseDto>? RoomSellers { get; set; }
    public MediaAttachmentResponseDto? MediaAttachment { get; set; }

    public RoomResponseDto()
    {

    }
    public RoomResponseDto(
        Guid id,
        string name,
        Guid ownerId,
        OnlineHouseResponseDto? owner,
        IEnumerable<RoomSellerResponseDto> roomSellers,
        AccumulatedResponseDto accumulated,
        MediaAttachmentResponseDto? mediaAttachment
        )
    {
        Id = id;
        Name = name;
        OwnerId = ownerId;
        Owner = owner;
        RoomSellers = roomSellers;
        Accumulated = accumulated;
        MediaAttachment = mediaAttachment;
    }
    internal static RoomResponseDto ConvertToDto(Room room)
    {
        var roomSellerResponse = room.RoomsSellers?.Select(r => RoomSellerResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<RoomSellerResponseDto>();
        var ownerResponse = room.Owner != null ? OnlineHouseResponseDto.ConvertToDto(room.Owner) : null;
        var accumulatedResponse = room.Accumulated != null ? AccumulatedResponseDto.ConvertToDto(room.Accumulated) : null;
        var mediaResponse = MediaAttachmentResponseDto.ConvertToDto(room.MediaAttachment);

        return new RoomResponseDto(
            room.Id,
            room.Name,
            room.OwnerId,
            ownerResponse,
            roomSellerResponse,
            accumulatedResponse,
            mediaResponse
        );
    }
    internal static RoomResponseDto ConvertToDtoToOnlineHouse(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            Name = room.Name
        };


    }
    internal static RoomResponseDto ConvertToSocketDto(Room room)
    {
        var roomSellerResponse = room.RoomsSellers?.Select(r => RoomSellerResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<RoomSellerResponseDto>();
        var ownerResponse = room.Owner != null ? OnlineHouseResponseDto.ConvertToDto(room.Owner) : null;
        return new RoomResponseDto(
            room.Id,
            room.Name,
            room.OwnerId,
            ownerResponse,
            roomSellerResponse,
            null,
            null
        );
    }
}
