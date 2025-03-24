using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Entities;

public class Seller : Entity, ITransactionParticipant
{
    public decimal Balance { get; set; }
    public decimal PrizeBalance { get ; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DateBirth { get; set; }
    public decimal Comission { get; set; }
    public IEnumerable<Punter> Punters { get; set; }
    public IEnumerable<RoomSeller> Rooms { get; set; }
    public IEnumerable<Room> OwnerRooms { get; set; }
   

    public Seller()
    {

    }
    public Seller(string email, string cpf, DateTime datebirth, decimal comission)
    {
        this.Email = email;
        this.Cpf = cpf;
        this.DateBirth = datebirth;
        this.Comission = comission;
    }
}