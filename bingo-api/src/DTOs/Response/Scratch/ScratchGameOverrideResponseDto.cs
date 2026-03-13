using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchGameOverrideResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Subtitle { get; set; }

    public decimal CardValue { get; set; }

    public Guid OnlineHouseId { get; set; }

    public Guid ScratchGameId { get; set; }

    public string? ScratchGameName { get; set; }

    public static ScratchGameOverrideResponseDto ConvertToDto(ScratchGameOverride entity)
    {
        return new ScratchGameOverrideResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Subtitle = entity.Subtitle,
            CardValue = entity.CardValue,
            OnlineHouseId = entity.OnlineHouseId,
            ScratchGameId = entity.ScratchGameId,
            ScratchGameName = entity.ScratchGame?.Name
        };
    }
}
