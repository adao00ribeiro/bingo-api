
namespace bingo_api.src.DTOs.Request;

public record ForgotPasswordRequestDto
{
    public string Email { get; set; } = null!;
}
