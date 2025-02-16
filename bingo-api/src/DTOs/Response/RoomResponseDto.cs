
using bingo_api.src.Entities;


namespace bingo_api.src.DTOs.Response;

public record RoomResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid OwnerId { get; set; }

    public SellerResponseDto? Owner { get; set; }
    public IEnumerable<RoomSellerResponseDto>? RoomSellers { get; set; }
    public RoomResponseDto(Guid id, string name, Guid ownerId, SellerResponseDto? owner, IEnumerable<RoomSellerResponseDto> roomSellers)
    {
        Id = id;
        Name = name;
        OwnerId = ownerId;
        Owner = owner;
        RoomSellers = roomSellers;
    }
    internal static RoomResponseDto ConvertToDto(Room room)
    {
        var roomSellerResponse = room.RoomsSellers?.Select(r => RoomSellerResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<RoomSellerResponseDto>();
        var ownerResponse = room.Owner != null ? SellerResponseDto.ConvertToDto(room.Owner) : null;
        return new RoomResponseDto(
            room.Id,
            room.Name,
            room.OwnerId,
            ownerResponse,
            roomSellerResponse
        );
    }
}
