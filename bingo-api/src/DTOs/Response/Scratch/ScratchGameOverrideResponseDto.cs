using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch;

public record ScratchGameOverrideResponseDto : EntityResponseDto
{
    public string Title { get; set; }

    public string Subtitle { get; set; }

    public decimal CardValue { get; set; }

    public Guid OnlineHouseId { get; set; }

    public Guid ScratchGameId { get; set; }


    public ScratchGameOverrideResponseDto(
        Guid id,
      string title,
      string subtitle,
       decimal cardValue,
        Guid onlineHouseId,
        Guid scratchGameId,
          DateTime CreatedAt,
      DateTime UpdatedAt
        ) : base(id, CreatedAt, UpdatedAt)
    {
        Title = title;
        Subtitle = subtitle;
        CardValue = cardValue;
        OnlineHouseId = onlineHouseId;
        ScratchGameId = scratchGameId;
    }

    public static ScratchGameOverrideResponseDto ConvertToDto(ScratchGameOverride entity)
    {
        return new ScratchGameOverrideResponseDto(
            
            id:  entity.Id,
            title : entity.Title,
            subtitle : entity.Subtitle,
            cardValue : entity.CardValue,
            onlineHouseId : entity.OnlineHouseId,
            scratchGameId : entity.ScratchGameId,
            CreatedAt : entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
      
    }
}
