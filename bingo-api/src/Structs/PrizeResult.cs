using bingo_api.src.Enums;

namespace bingo_api.src.Structs;

public class PrizeResult
{
    public Guid PrizeId { get; set; }
    public decimal Value { get; set; }
    public Guid RoundId { get; set; }
    public EPrizeType PrizeType { get; set; }
    public List<WinningCardsInfo> WinningCards { get; set; }
    public List<TopCardInfo> ListTopCards { get; set; }
}
