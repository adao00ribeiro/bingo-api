
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Request.Blockchain;

public record TokenRequestDto
{
    public string Symbol { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Decimals { get; set; }
    public bool IsNative { get; set; }

    internal static Token ConvertToEntity(TokenRequestDto dto)
    {
        return new Token(dto.Symbol, dto.Name, dto.Decimals , dto.IsNative);
    }
}
