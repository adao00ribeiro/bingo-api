using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchSellerGameResponseDto
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid ScratchGameId { get; set; }
    public SellerResponseDto? Seller { get; set; }
    public ScratchGameResponseDto? ScratchGame { get; set; }
    internal static ScratchSellerGameResponseDto ConvertToDto(ScratchSellerGame entity)
    {
        return new ScratchSellerGameResponseDto
        {
            Id = entity.Id,
            SellerId = entity.SellerId,
            ScratchGameId = entity.ScratchGameId,
            Seller = entity.Seller is null ? null : SellerResponseDto.ConvertToDto(entity.Seller),
            ScratchGame = entity.ScratchGame is null ? null : ScratchGameResponseDto.ConvertToDto(entity.ScratchGame)
        };
    }
}
