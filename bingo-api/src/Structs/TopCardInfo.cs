using bingo_api.src.DTOs.Response;
using Bogus.DataSets;


namespace bingo_api.src.Structs;

public class TopCardInfo
{
    public TopCardInfo()
    {

    }
    public CardResponseDto Card { get; set; }
    public List<int> MissingNumbers { get; set; }
    public int Hits { get; set; }
    public DateTime CreatedAt{ get; set; }
}
