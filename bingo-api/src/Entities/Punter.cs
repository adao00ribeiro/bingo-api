using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class Punter : Entity
{
    public decimal Balance { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public DateTime DateBirth { get; set; }
    public  IEnumerable<Card> Cards { get; set; }
    public Guid SellerId { get; set; }
    public  Seller? Seller { get; set; }
    public  IEnumerable<Recharge>? Recharges { get; set; }
    public Punter()
    {

    }
    public Punter(string email, string name, string cpf, DateTime datebirth, Guid sellerId)
    {
        this.Email = email;
        this.Name = name;
        this.Cpf = cpf;
        this.DateBirth = datebirth;
        this.SellerId = sellerId;
    }
}
