using bingo_api.src.Entities.Scratch;
using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.Helpers.Scratchcard;

public class CardGenerator
{
    private readonly ScratchGameOverride _modalityOverride;
    private readonly ScratchGame _modality;

    public CardGenerator(ScratchGameOverride modalityOverride)
    {
        _modalityOverride = modalityOverride ?? throw new ArgumentNullException(nameof(modalityOverride));
        _modality = _modalityOverride.ScratchGame ?? throw new ArgumentNullException("Modality not set");
    }

    public CardGenerationResult Generate(decimal totalSales, decimal totalPrizes)
    {
        var rtpEngine = new RtpEngine(
            rtp: _modality.Rtp,
            cardValue: _modalityOverride.CardValue,
            payout: _modality.PayoutTable,
            totalSales: totalSales,
            totalPrizes: totalPrizes
        );

        var rtpResult = rtpEngine.Run();
        decimal prize = rtpResult.Prize;

        List<ScratchArea> areas = prize > 0 ? BuildWinningCard(prize) : BuildLosingCard();

        return new CardGenerationResult
        {
            Prize = prize,
            Areas = areas,
            TotalSales = rtpResult.TotalSales,
            TotalPrizes = rtpResult.TotalPrizes,
            Rtp = rtpResult.Rtp
        };
    }

    private List<ScratchArea> BuildWinningCard(decimal prize)
    {
        int winningSymbol = SymbolForPrize(prize);
        int qtyToAward = _modality.QuantityToAward;

        var symbols = Enumerable.Repeat(winningSymbol, qtyToAward).ToList();
        var fillers = SafeFillers(_modality.Rows * _modality.Cols - qtyToAward, winningSymbol);

        var allSymbols = symbols.Concat(fillers).OrderBy(_ => Guid.NewGuid()).ToList();
        return BuildAreas(allSymbols);
    }

    private List<ScratchArea> BuildLosingCard()
    {
        int totalAreas = _modality.Rows * _modality.Cols;
        var rand = new Random();

        while (true)
        {
            var symbols = Enumerable.Range(0, totalAreas).Select(_ => rand.Next(1, 11)).ToList();
            if (!WinningCombo(symbols)) return BuildAreas(symbols);
        }
    }

    private List<int> SafeFillers(int count, int winningSymbol)
    {
        var rand = new Random();
        var fillers = new List<int>();

        while (fillers.Count < count)
        {
            int candidate = rand.Next(1, 11);
            var temp = fillers.Append(candidate).ToList();

            int effective = temp.Count(x => x == candidate);
            if (candidate == winningSymbol) effective += _modality.QuantityToAward;

            if (effective < _modality.QuantityToAward) fillers.Add(candidate);
        }

        return fillers;
    }

    private bool WinningCombo(List<int> symbols)
    {
        return symbols.GroupBy(x => x).Any(g => g.Count() >= _modality.QuantityToAward);
    }

    private int SymbolForPrize(decimal prize)
    {
        for (int i = 0; i < _modality.PayoutTable.Count; i++)
        {
            var m = _modality.PayoutTable[i];
            if (Math.Round(_modalityOverride.CardValue * (decimal)m.Multiplier, 2) == prize)
                return i + 1;
        }
        return 1;
    }

    private List<ScratchArea> BuildAreas(List<int> symbols)
    {
        return symbols.Select(s => new ScratchArea { Element = s, ScratchedAt = null }).ToList();
    }
}

public class CardGenerationResult
{
    public decimal Prize { get; set; }
    public List<ScratchArea> Areas { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPrizes { get; set; }
    public decimal Rtp { get; set; }
}
