using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;


public record ScratchGameAttributesDto
{
    public Dictionary<string, decimal> PayoutTable { get; set; } = new();
    public List<ScratchSymbolDto> Symbols { get; set; } = new();

    internal static ScratchGameAttributesDto ConvertToDto(ScratchGameAttributes attributes)
    {
        return new ScratchGameAttributesDto
        {
            PayoutTable = attributes.PayoutTable,
            Symbols = attributes.Symbols
                .Select(ScratchSymbolDto.ConvertToDto)
                .ToList()
        };
    }
}
