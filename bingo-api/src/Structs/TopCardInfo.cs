using bingo_api.src.DTOs.Response;


namespace bingo_api.src.Structs;

public class TopCardInfo
{
    public TopCardInfo()
    {

    }
    public CardResponseDto Card { get; set; }
    public List<int> MissingNumbers { get; set; }
    public int Hits { get; set; }
}
