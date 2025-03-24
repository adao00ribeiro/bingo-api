using bingo_api.src.DTOs.Response;

namespace bingo_api.src.Structs;

public class WinningCardsInfo
{
    public PunterResponseDto Punter { get; set; }
    public CardResponseDto Card { get; set; }
    public decimal ValueOfEachWinner { get; set; }


}
