using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Scratch;

public class ScratchTicket : Entity
{
    public int Multiplier { get; set; } = 1;
    public decimal PrizeWon { get; set; } = 0;//valor ganho com o bilhete
    public bool Revealed { get; set; } = false;
    public ScratchTicketAttributes Attributes { get; set; } = new();
    public Guid ScratchSellerGameId { get; set; }
    public ScratchSellerGame ScratchSellerGame { get; set; }
    public Guid? ScratchPrizeId { get; set; }
    public ScratchPrize ScratchPrize { get; set; }
    public Guid? ScratchBuyId { get; set; }
    public ScratchBuy ScratchBuy { get; set; }
}
public class ScratchTicketAttributes
{
    public Guid PunterId { get; set; }
    public List<ScratchItem> Items { get; set; } = new();
}
public class ScratchItem
{
    public string Name { get; set; }
    public int Position { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public bool IsWinner { get; set; } 
}