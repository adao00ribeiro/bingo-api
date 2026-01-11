using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Entities;

public class Punter : Entity, ITransactionParticipant
{
    public decimal Balance { get; set; }
    public decimal PrizeBalance { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    [JsonIgnore]
    public bool IsBot { get; set; }
    public DateTime DateBirth { get; set; }
    public IEnumerable<Card> Cards { get; set; }
    public Guid OnlineHouseId { get; set; }
    public OnlineHouse OnlineHouse { get; set; }
    public IEnumerable<Recharge>? Recharges { get; set; }
    public IEnumerable<PunterWithdrawal> Withdrawals { get; set; }
    public string IndicateTag { get; set; }
    public string? RegisteredWithTag { get; set; } // usado só uma vez
    public IEnumerable<ScratchTicket> ScratchTickets { get; set; }
    public Punter()
    {

    }
    public Punter(string email, string name, string cpf, DateTime datebirth, Guid onlineHouseId, string RegisteredWithTag)
    {
        this.Email = email;
        this.Name = name;
        this.Cpf = NormalizeCpf(cpf);
        this.DateBirth = datebirth;
        this.OnlineHouseId = onlineHouseId;
        this.IndicateTag = "";
        this.RegisteredWithTag = RegisteredWithTag;
    }
    public Punter(string email, string name, string cpf, DateTime datebirth, Guid onlineHouseId, bool isBot)
    {
        this.Email = email;
        this.Name = name;
        this.Cpf =  NormalizeCpf(cpf);
        this.DateBirth = datebirth;
        this.OnlineHouseId = onlineHouseId;
        this.IndicateTag = "";
        this.RegisteredWithTag = "";
        this.IsBot = isBot;
    }

     private static string NormalizeCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório");

        var onlyNumbers = Regex.Replace(cpf, @"\D", "");

        if (onlyNumbers.Length != 11)
            throw new ArgumentException("CPF inválido");

        return onlyNumbers;
    }
}
