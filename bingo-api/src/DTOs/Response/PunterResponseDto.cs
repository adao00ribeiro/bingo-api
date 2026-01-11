
using System.Text.Json.Serialization;
using bingo_api.src.DTOs.Response.Bingo;
using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;
namespace bingo_api.src.DTOs.Response;

public record PunterResponseDto : EntityResponseDto
{
    public string Name { get; set; }
    public string Cpf { get; set; }
    public decimal Balance { get; set; }
    public decimal PrizeBalance { get; set; }
    public IEnumerable<CardResponseDto>? Cards { get; set; }
    public Guid OnlineHouseId { get; set; }
    public OnlineHouseResponseDto OnlineHouse { get; set; }
    public UserResponseDto user { get; set; }

    [JsonIgnore]
    public IEnumerable<RechargeResponseDto>? Recharges { get; set; }
    public PunterResponseDto(
        Guid id,
        string name,
        string cpf,
        decimal balance,
        decimal prizeBalance,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        IEnumerable<CardResponseDto> cards,
        Guid onlineHouseId,
        OnlineHouseResponseDto onlineHouse,
        IEnumerable<RechargeResponseDto> recharges)
    : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Name = name;
        Cpf = cpf;
        Balance = balance;
        PrizeBalance = prizeBalance;
        Cards = cards;
        OnlineHouseId = onlineHouseId;
        OnlineHouse = onlineHouse;
        Recharges = recharges;

    }
    internal static PunterResponseDto ConvertToDto(Punter punter)
    {

        var onlineHouseResponse = punter.OnlineHouse != null ? OnlineHouseResponseDto.ConvertToDto(punter.OnlineHouse) : null;
        var RechargeResponse = punter.Recharges?.Select(r => RechargeResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<RechargeResponseDto>();
        return new PunterResponseDto(
            punter.Id,
            punter.Name,
            punter.Cpf,
            punter.Balance,
            punter.PrizeBalance,
            punter.CreatedAt,
            punter.UpdatedAt,
            null,
            punter.OnlineHouseId,
            onlineHouseResponse,
            RechargeResponse
        );
    }
    internal static PunterResponseDto ConvertToSocketDto(Punter punter)
    {

        return new PunterResponseDto(
            punter.Id,
            punter.Name,
            "",
            0,
            0,
            punter.CreatedAt,
            punter.UpdatedAt,
            null,
            punter.OnlineHouseId,
            null,
            Enumerable.Empty<RechargeResponseDto>()
        );
    }
}
