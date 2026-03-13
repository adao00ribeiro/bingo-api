using bingo_api.src.Structs.Scratchcard;

namespace bingo_api.src.Helpers.Scratchcard;

public class RtpEngine
{
    private readonly double _rtp;
    private readonly decimal _cardValue;
    private readonly List<ScratchPayout> _payout;
    private decimal _totalSales;
    private decimal _totalPrizes;

    public RtpEngine(
        double rtp,
        decimal cardValue,
        List<ScratchPayout> payout,
        decimal totalSales,
        decimal totalPrizes)
    {
        _rtp = rtp;
        _cardValue = cardValue;
        _payout = payout;
        _totalSales = totalSales;
        _totalPrizes = totalPrizes;
    }

    public RtpResult Run()
    {
        if (_cardValue <= 0) throw new ArgumentException("Invalid card value");
        if (_rtp <= 0) throw new ArgumentException("Invalid probability");

        _totalSales += _cardValue;
        decimal prize = SelectPrize();
        _totalPrizes += prize;

        return new RtpResult
        {
            Prize = prize,
            TotalSales = _totalSales,
            TotalPrizes = _totalPrizes,
            Rtp = _totalSales == 0 ? 0 : _totalPrizes / _totalSales
        };
    }

    private decimal SelectPrize()
    {
        decimal limitPrize = (_totalSales - _totalPrizes) * (decimal)_rtp;
        limitPrize = Math.Max(limitPrize, 0);
        int attempts = 0;

        while (attempts < 150)
        {
            var candidate = WeightedSample();
            if (candidate <= limitPrize) return candidate;
            attempts++;
        }

        return 0;
    }

    private decimal WeightedSample()
    {
        var roll = new Random().NextDouble();
        double cumulative = 0.0;

        foreach (var entry in _payout)
        {
            double probability = entry.Probability;
            double multiplier = entry.Multiplier;

            cumulative += probability;
            if (roll < cumulative)
            {
                return Math.Round(_cardValue * (decimal)multiplier, 2);
            }
        }

        // fallback
        var last = _payout.Last();
        return Math.Round(_cardValue * (decimal)last.Multiplier, 2);
    }
}
public class RtpResult
{
    public decimal Prize { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPrizes { get; set; }
    public decimal Rtp { get; set; }
}