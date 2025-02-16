using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class CardWinner : Entity
{
    public decimal Value { get; set; }
    public Guid CardId { get; set; }
    public Card? Card { get; set; }
    public Guid PrizeId { get; set; }
    public Prize? Prize { get; set; }

    public CardWinner(decimal value, Guid cardId, Guid prizeId)
    {
        Value = value;
        CardId = cardId;
        PrizeId = prizeId;
    }

}
