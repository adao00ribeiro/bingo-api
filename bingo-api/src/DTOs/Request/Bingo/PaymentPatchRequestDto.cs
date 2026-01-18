namespace bingo_api.src.DTOs.Request.Bingo;

public record PaymentPatchRequestDto
{
    public string? Token { get; set; } // Usado no PushPay
    public string? PixPayload { get; set; } // Copia e Cola
}
