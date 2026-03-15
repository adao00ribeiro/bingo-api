using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchGameOverride : Entity
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public decimal CardValue { get; set; }
    public OnlineHouse OnlineHouse { get; set; }
    public Guid OnlineHouseId { get; set; }
    public Guid ScratchGameId { get; set; }
    public ScratchGame ScratchGame { get; set; }
    public ICollection<ScratchBuy> ScratchBuys { get; set; }

    public ScratchGameOverride(string title, string subtitle, decimal cardValue, Guid onlineHouseId, Guid scratchGameId)
    {
        Title = title;
        Subtitle = subtitle;
        CardValue = cardValue;
        OnlineHouseId = onlineHouseId;
        ScratchGameId = scratchGameId;
    }

}
