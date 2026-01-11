using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseToPunterDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<RoomResponseDto> Rooms { get; set; }


    public SellerResponseToPunterDto(Guid id, string email)
    {
        Id = id;
        Email = email;
    }
    internal static SellerResponseToPunterDto ConvertToDtoInPunter(Seller seller)
    {
        
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email
        

        );
    }
    internal static SellerResponseToPunterDto ConvertToDto(Seller seller)
    {
        return new SellerResponseToPunterDto(
            seller.Id,
            seller.Email

        );
    }
}
