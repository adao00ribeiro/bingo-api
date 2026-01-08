using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Response.Blockchain;

public record TokenResponseDto
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Decimals { get; set; }
    public bool IsNative { get; set; }
    public IEnumerable<TokenAddressResponseDto> TokenAddresses { get; set; } = null!;
    internal static TokenResponseDto ConvertToDto(Token token)
    {
        // var tokenAddressResponse = token.TokenAddresses?.Select(x => TokenAddressResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<TokenAddressResponseDto>();
        return new TokenResponseDto
        {
            Id = token.Id,
            Symbol = token.Symbol,
            Name = token.Name,
            Decimals = token.Decimals,
            IsNative = token.IsNative,
            //TokenAddresses = tokenAddressResponse
        };
    }
}
