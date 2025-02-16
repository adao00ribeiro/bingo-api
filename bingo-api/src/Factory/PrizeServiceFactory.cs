using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories.Shared;
using bingo_api.src.Interfaces.Services;
using bingo_api.src.Services;
using bingo_api.src.Services.Prizes;

namespace bingo_api.src.Factory;

public static class PrizeServiceFactory
{
    public static IPrizeService CreateService(Prize prize)
    {
        return prize.Type switch
        {
            EPrizeType.FourInLine => new PrizeFourInLineService(prize),
            EPrizeType.FourCorners => new PrizeFourCornersService(prize),
            EPrizeType.SingleLine => new PrizeSingleLineService(prize),
            EPrizeType.SingleColumn => new PrizeSingleColumnService(prize),
            EPrizeType.Diagonal => new PrizeDiagonalService(prize),
            EPrizeType.InvertedDiagonal => new PrizeInvertedDiagonalService(prize),
            EPrizeType.DoubleLine => new PrizeDoubleLineService(prize),
            EPrizeType.DoubleColumn => new PrizeDoubleColumnService(prize),
            EPrizeType.FullCard => new PrizeFullCardService(prize),
            EPrizeType.TShape => new PrizeTShapeService(prize),
            EPrizeType.XShape => new PrizeXShapeService(prize),
            EPrizeType.PlusShape => new PrizePlusShapeService(prize),
            EPrizeType.OuterEdge => new PrizeOuterEdgeService(prize),
            _ => throw new ArgumentException("Tipo de prêmio desconhecido", nameof(prize.Type))
        };
    }
}
