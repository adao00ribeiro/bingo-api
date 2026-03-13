
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Request.Scratch;

public record ScratchGameOverrideRequestDto
{
    public string Title { get; init; }
    public string Subtitle { get; init; }
    public decimal CardValue { get; init; }
    public Guid OnlineHouseId  { get; set; }
    public Guid ScratchGameId  { get; set; }

    internal static ScratchGameOverride ConvertToEntity(ScratchGameOverrideRequestDto request)
    {
        return new ScratchGameOverride(request.OnlineHouseId, request.ScratchGameId);
    }
}
