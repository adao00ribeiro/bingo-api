using bingo_api.src.Entities.Shared;
using bingo_api.src.Exceptions;
using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.Entities.Scratch;

public class ScratchTicket : Entity
{
    public decimal Value { get; set; } 
    public ScratchTicketAttributes Attributes { get; set; } = new();
    public List<ScratchPrize> ScratchPrizes { get; set; }
    public Guid ScratchBuyId { get; set; }
    public ScratchBuy ScratchBuy { get; set; }


    public ScratchTicket(decimal value ,Guid scratchBuyId  )
    {
        Value = value;
        ScratchBuyId = scratchBuyId;
    }

     /// <summary>
    /// Cria uma nova cartela a partir de dados gerados pelo <see cref="CardGenerator"/>.
    /// </summary>
    public static ScratchTicket Create(ScratchBuy scratchBuy, decimal value, IEnumerable<ScratchArea> areas)
    {
        ArgumentNullException.ThrowIfNull(scratchBuy);
        if (value <= 0) throw new DomainException("O valor da cartela deve ser maior que zero.");

        var card = new ScratchTicket(value,scratchBuy.Id);
       

        card.Attributes.Areas.AddRange(areas);
        return card;
    }

    // ─── Queries ──────────────────────────────────────────────────────────────

    public bool IsWinner => ScratchPrizes.Count > 0;

    public bool AllScratched =>
        Attributes.Areas.Count > 0 && Attributes.Areas.All(a => a.ScratchedAt.HasValue);

    public IReadOnlyList<ScratchArea> ScratchedAreas =>
        Attributes.Areas.Where(a => a.ScratchedAt.HasValue).ToList();

    public bool WinningCombinationRevealed(int quantityToAward)
    {
        var tally = ScratchedAreas
            .GroupBy(a => a.Element)
            .ToDictionary(g => g.Key, g => g.Count());

        return tally.Values.Any(count => count >= quantityToAward);
    }

    // ─── Commands ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Revela uma área da cartela. Lança <see cref="DomainException"/> se a operação for inválida.
    /// </summary>
    public void ScratchArea(int areaIndex, DateTime scratchedAt)
    {
        if (areaIndex < 0 || areaIndex >= Attributes.Areas.Count)
            throw new DomainException($"Índice de área inválido: {areaIndex}.");

        var area = Attributes.Areas[areaIndex];

        if (area.ScratchedAt.HasValue)
            throw new DomainException($"A área {areaIndex} já foi revelada.");

        Attributes.Areas[areaIndex] = area with { ScratchedAt = scratchedAt };

       // AddDomainEvent(new AreaScratchedEvent(Id, areaIndex));
    }

    /// <summary>
    /// Registra um prêmio para a cartela. Chamado pelo serviço de premiação após confirmar vitória.
    /// </summary>
    public void AwardPrize(decimal prizeValue)
    {
        if (IsWinner)
            throw new DomainException("Esta cartela já possui um prêmio registrado.");

        ScratchPrizes.Add(new ScratchPrize(prizeValue,Id));

      //  AddDomainEvent(new PrizeAwardedEvent(Id, prizeValue));
    }
}
