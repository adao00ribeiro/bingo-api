using bingo_api.src.DTOs.Response.Bingo;
using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseDto : EntityResponseDto
{
    public decimal Balance { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DateBirth { get; set; }
    public decimal Comission { get; set; }
    public OnlineHouseResponseDto OnlineHouse { get; set; }
    public SellerResponseDto(Guid id, decimal balance, string email, string cpf, DateTime dateBirth, decimal comission, OnlineHouseResponseDto onlineHouse, DateTime CreatedAt,
        DateTime UpdatedAt)
    : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Balance = balance;
        Email = email;
        Cpf = cpf;
        DateBirth = dateBirth;
        Comission = comission;
        OnlineHouse = onlineHouse;
    }

    internal static SellerResponseDto ConvertToDto(Seller seller)
    {
        var onlineReponse = seller.OnlineHouse != null ? OnlineHouseResponseDto.ConvertToDto(seller.OnlineHouse) : null;
        return new SellerResponseDto(
            seller.Id,
            seller.Balance,
            seller.Email,
            seller.Cpf,
            seller.DateBirth,
            seller.Comission,
            onlineReponse,
            seller.CreatedAt,
            seller.UpdatedAt
        );
    }
}
