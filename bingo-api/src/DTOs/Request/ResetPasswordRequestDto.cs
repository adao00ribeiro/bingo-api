using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.DTOs.Request;

public record ResetPasswordRequestDto
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
