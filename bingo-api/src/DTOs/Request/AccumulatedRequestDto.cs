
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Request;

public class AccumulatedRequestDto
{
   
    internal static Accumulated ConvertToEntity(AccumulatedRequestDto dto)
    {
        return new Accumulated();
    }
}
