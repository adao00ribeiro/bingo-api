using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Extensions.Seeds;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;


public record ScratchGameAttributesDto
{
    public List<ScratchPayoutDto> PayoutTable { get; set; }
    public List<ScratchSymbolDto> Symbols { get; set; }

    internal static ScratchGameAttributesDto ConvertToDto(ScratchGameAttributes attributes)
    {
        return new ScratchGameAttributesDto
        {
            PayoutTable = [.. attributes.PayoutTable.Select(ScratchPayoutDto.ConvertToDto)],
            Symbols = [.. attributes.Symbols.Select(ScratchSymbolDto.ConvertToDto)]
        };
    }
}
