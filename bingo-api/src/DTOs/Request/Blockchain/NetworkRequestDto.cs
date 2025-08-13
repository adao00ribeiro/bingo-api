
using bingo_api.src.DTOs.Response.Blockchain;
using bingo_api.src.Entities.Blockchain;

namespace bingo_api.src.DTOs.Request.Blockchain;

public record NetworkRequestDto
{
    public string Name { get; set; } = null!;
    public string RpcUrl { get; set; } = null!;
    public int ChainId { get; set; }
    internal static Network ConvertToEntity(NetworkRequestDto dto)
    {
        return new Network(dto.Name, dto.RpcUrl, dto.ChainId);
    }
}
