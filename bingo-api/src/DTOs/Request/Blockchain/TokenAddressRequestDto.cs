
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Request.Blockchain;

public record TokenAddressRequestDto
{
    public Guid TokenId { get; set; }
    public Guid NetworkId { get; set; }
    public string ContractAddress { get; set; } = null!;

    internal static TokenAddress ConvertToEntity(TokenAddressRequestDto dto)
    {
        return new TokenAddress(dto.TokenId, dto.NetworkId, dto.ContractAddress);
    }

}
