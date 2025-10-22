
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Request.Scratch;

public record ScratchSellerGameRequestDto
{
    public Guid SellerId { get; set; }
    public Guid ScratchGameId { get; set; }

    internal static ScratchSellerGame ConvertToEntity(ScratchSellerGameRequestDto request)
    {
         return new ScratchSellerGame(request.SellerId , request.ScratchGameId);
    }
}
