using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request;

public record DeactivateAccountRequestDto
{
    public string UserId { get; set; }
}
