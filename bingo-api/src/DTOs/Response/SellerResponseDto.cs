using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseDto : EntityResponseDto
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DateBirth { get; set; }
    public decimal Comission { get; set; }
    public SellerSettings Settings { get; set; }
    public IEnumerable<Punter> Punters { get; set; }
    public IEnumerable<RoomSeller> Rooms { get; set; }
    public IEnumerable<Room> OwnerRooms { get; set; }

    public SellerResponseDto(Guid id, decimal balance, string email, string cpf, DateTime dateBirth, decimal comission, SellerSettings settings, DateTime CreatedAt,
        DateTime UpdatedAt)
    : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Balance = balance;
        Email = email;
        Cpf = cpf;
        DateBirth = dateBirth;
        Comission = comission;
        Settings = settings;
    }

    internal static SellerResponseDto ConvertToDto(Seller seller)
    {
        return new SellerResponseDto(
            seller.Id,
            seller.Balance,
            seller.Email,
            seller.Cpf,
            seller.DateBirth,
            seller.Comission,
            seller.Settings,
            seller.CreatedAt,
            seller.UpdatedAt
        );
    }
}
