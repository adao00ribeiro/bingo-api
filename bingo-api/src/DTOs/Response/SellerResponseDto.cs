using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response;

public record SellerResponseDto
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DateBirth { get; set; }
    public decimal Comission { get; set; }
    public IEnumerable<Punter> Punters { get; set; }
    public IEnumerable<RoomSeller> Rooms { get; set; }
    public IEnumerable<Room> OwnerRooms { get; set; }

    public SellerResponseDto(Guid id, decimal balance, string email, string cpf, DateTime dateBirth, decimal comission)
    {
        Id = id;
        Balance = balance;
        Email = email;
        Cpf = cpf;
        DateBirth = dateBirth;
        Comission = comission;
    }

    internal static SellerResponseDto ConvertToDto(Seller seller)
    {
        return new SellerResponseDto(
            seller.Id,
            seller.Balance,
            seller.Email,
            seller.Cpf,
            seller.DateBirth,
            seller.Comission
        );
    }
}
