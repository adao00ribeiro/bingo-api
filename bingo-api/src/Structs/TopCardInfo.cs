using bingo_api.src.DTOs.Request;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities;

namespace bingo_api.src.Structs;

public class TopCardInfo
{
    public TopCardInfo()
    {

    }

    public CardResponseDto Card { get; set; }
    public PunterResponseDto Punter { get; set; }
    public List<int> MissingNumbers { get; set; }
    public int Hits { get; set; }
}
