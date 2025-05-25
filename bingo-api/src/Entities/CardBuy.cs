using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities;

public class CardBuy : Entity
{
    public int Quantity { get; set; }
    public Guid RoundId { get; set; }
    public Guid PunterId { get; set; }
    public virtual IEnumerable<Card> Cards { get; set; }
    public CardBuy(int quantity, Guid roundId, Guid punterId)
    {
        Quantity = quantity;
        RoundId = roundId;
        PunterId = punterId;
    }
}
