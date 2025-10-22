using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Entities.Scratch;

namespace bingo_api.src.DTOs.Response.Scratch.Jsonb;

public record ScratchItemDto
{
    public string Name { get; set; }
    public int Position { get; set; }
    public string Symbol { get; set; }
    public bool IsWinner { get; set; }

    internal static ScratchItemDto ConvertToDto(ScratchItem entity)
    {
        return new ScratchItemDto
        {
            Name = entity.Name,
            Position = entity.Position,
            Symbol = entity.Symbol,
            IsWinner = entity.IsWinner
        };
    }
}