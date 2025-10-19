using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseToPunterDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<RoomResponseDto> Rooms { get; set; }

    public SellerSettings Settings { get; set; }

    public SellerResponseToPunterDto(Guid id, string email, IEnumerable<RoomResponseDto> rooms , SellerSettings settings)
    {
        Id = id;
        Email = email;
        Rooms = rooms;
        Settings = settings;
    }
    internal static SellerResponseToPunterDto ConvertToDtoInPunter(Seller seller)
    {
        var roomResponse = seller.OwnerRooms?.Select(x => RoomResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<RoomResponseDto>();
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email,
            roomResponse,
            seller.Settings

        );
    }
    internal static SellerResponseToPunterDto ConvertToDto(Seller seller)
    {
        var roomResponse = seller.OwnerRooms?.Select(x => RoomResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<RoomResponseDto>();
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email,
            roomResponse,
            seller.Settings

        );
    }
}
