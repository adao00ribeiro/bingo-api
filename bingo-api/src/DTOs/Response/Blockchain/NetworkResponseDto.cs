using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Response.Blockchain;

public record NetworkResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string RpcUrl { get; set; } = null!;
    public int ChainId { get; set; }
    public IEnumerable<TokenAddressResponseDto> TokenAddresses { get; set; } = null!;

    internal static NetworkResponseDto ConvertToDto(Network network)
    {
        var tokenAddressesResponse = network.TokenAddresses?.Select(x => TokenAddressResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<TokenAddressResponseDto>();
        return new NetworkResponseDto
        {
            Id = network.Id,
            Name = network.Name,
            RpcUrl = network.RpcUrl,
            ChainId = network.ChainId,
        };
    }
}
