using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseToPunterDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<RoomResponseDto> Rooms { get; set; }
    public SellerResponseToPunterDto(Guid id, string email, IEnumerable<RoomResponseDto> rooms)
    {
        Id = id;
        Email = email;
        Rooms = rooms;
    }
    internal static SellerResponseToPunterDto ConvertToDtoInPunter(Seller seller)
    {
        var roomResponse = seller.OwnerRooms?.Select(x => RoomResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<RoomResponseDto>();
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email,
            roomResponse
        );
    }
    internal static SellerResponseToPunterDto ConvertToDto(Seller seller)
    {
        var roomResponse = seller.OwnerRooms?.Select(x => RoomResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<RoomResponseDto>();
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email,
            roomResponse
        );
    }
}
