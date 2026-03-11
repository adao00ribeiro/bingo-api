using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchSellerGame : Entity
{
    public Guid OnlineHouseId { get; set; }
    public Guid ScratchGameId { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public decimal CardValue { get; set; }
    public OnlineHouse OnlineHouse { get; set; }
    public ScratchGame ScratchGame { get; set; }
    public ICollection<ScratchBuy> ScratchBuys { get; set; }
    public ScratchSellerGame(Guid onlineHouseId, Guid scratchGameId)
    {
        OnlineHouseId = onlineHouseId;
        ScratchGameId = scratchGameId;
    }
}
