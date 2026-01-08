using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Response.Blockchain;

public record TokenAddressResponseDto
{
    public Guid Id { get; set; }
    public Guid TokenId { get; set; }
    public Guid NetworkId { get; set; }
    public string ContractAddress { get; set; } = null!;
    public TokenResponseDto Token { get; set; } = null!;
    public NetworkResponseDto Network { get; set; } = null!;


    internal static TokenAddressResponseDto ConvertToDto(TokenAddress tokenAddress, bool includeNetwork = true)
    {
        var tokenResponse = tokenAddress.Token != null ? TokenResponseDto.ConvertToDto(tokenAddress.Token) : null;
        var networkResponse = includeNetwork && tokenAddress.Network != null
                ? NetworkResponseDto.ConvertToDto(tokenAddress.Network, includeTokenAddresses: false)
                : null;

        return new TokenAddressResponseDto
        {
            Id = tokenAddress.Id,
            TokenId = tokenAddress.TokenId,
            NetworkId = tokenAddress.NetworkId,
            ContractAddress = tokenAddress.ContractAddress,
            Token = tokenResponse,
            Network = networkResponse,
        };
    }
}
