using bingo_api.src.Entities.Shared;
using bingo_api.src.Exceptions;

namespace bingo_api.src.Entities.Scratch;

public class ScratchBuy : Entity
{
    public decimal Value { get; set; }
    public int Quantity { get; set; }
    public Guid ScratchGameOverrideId { get;  set; }
    public ScratchGameOverride  ScratchGameOverride { get; set; }
    public Punter Punter { get; set; }
    public Guid PunterId { get;  set; }
   public ICollection<ScratchTicket> ScratchTickets{ get;  set; }= new List<ScratchTicket>();
   public ICollection<TransactionHistory> TransactionHistories{ get;  set; }
    public ScratchBuy()
    {
        
    }
    public ScratchBuy(int quantity, Guid scratchGameOverrideId, Guid punterId)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        Quantity = quantity;
        ScratchGameOverrideId = scratchGameOverrideId;
        PunterId = punterId;
    }

     /// <summary>
    /// Cria uma nova aposta. O saldo do revendedor é verificado antes da criação.
    /// </summary>
    public static ScratchBuy Create(
        ScratchGameOverride gameOverride,
        Punter punter,
        int quantity)
    {
        ArgumentNullException.ThrowIfNull(gameOverride);
        ArgumentNullException.ThrowIfNull(punter);

        if (quantity <= 0)
            throw new DomainException("A quantidade de cartelas deve ser maior que zero.");

        var totalValue = gameOverride.CardValue * quantity;

        var bet = new ScratchBuy
        {
            ScratchGameOverrideId = gameOverride.Id,
            ScratchGameOverride = gameOverride,
            PunterId = punter.Id,
            Punter = punter,
            Quantity = quantity,
            Value = totalValue
        };

       // bet.AddDomainEvent(new BetCreatedEvent(bet.Id, totalValue));
        return bet;
    }

    // ─── Mutações ─────────────────────────────────────────────────────────────

    public void AddTicket(ScratchTicket card)
    {
        ArgumentNullException.ThrowIfNull(card);
        ScratchTickets.Add(card);
    }
    public new bool IsDiscarded()
    {
       return DiscardedAt != null;
    }
    public new void Discard()
    {
        if (IsDiscarded())
            throw new DomainException("A aposta já foi descartada.");

        DiscardedAt = DateTime.UtcNow;
    }
}
