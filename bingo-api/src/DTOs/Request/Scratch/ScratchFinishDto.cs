using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request.Scratch;

public record ScratchFinishDto
{
    public Guid TicketId { get; init; }
}
