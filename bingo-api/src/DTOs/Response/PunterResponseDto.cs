
using System.Text.Json.Serialization;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;
namespace bingo_api.src.DTOs.Response;
public record PunterResponseDto
{
  

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public decimal Balance { get; set; }
    public IEnumerable<CardResponseDto>? Cards { get; set; }
    public Guid SellerId { get; set; }
    public SellerResponseToPunterDto? Seller { get; set; }

      public UserResponseDto user{ get; set; } 

    [JsonIgnore]
    public IEnumerable<RechargeResponseDto>? Recharges { get; set; }
    public PunterResponseDto(Guid id, string name, string cpf, decimal balance, IEnumerable<CardResponseDto> cards, Guid sellerId, SellerResponseToPunterDto? seller, IEnumerable<RechargeResponseDto> recharges)
    {
        Id = id;
        Name = name;
        Cpf = cpf;
        Balance = balance;
        Cards = cards;
        SellerId = sellerId;
        Seller = seller;
        Recharges = recharges;
    }
    internal static PunterResponseDto ConvertToDto(Punter punter)
    {

        var SellerResponse = punter.Seller != null ? SellerResponseToPunterDto.ConvertToDto(punter.Seller) : null;
        var RechargeResponse = punter.Recharges?.Select(r => RechargeResponseDto.ConvertToDto(r)) ?? Enumerable.Empty<RechargeResponseDto>();
        return new PunterResponseDto(
            punter.Id,
            punter.Name,
            punter.Cpf,
            punter.Balance,
            null,
            punter.SellerId,
            SellerResponse,
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
            null,
            punter.SellerId,
            null,
            Enumerable.Empty<RechargeResponseDto>()
        );
    }
}
