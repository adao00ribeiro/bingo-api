namespace bingo_api.src.DTOs.Request.Blockchain;

public record ConfirmDepositRequestDto
{
    public Guid DepositoId { get; set; }
    public string TxHash { get; set; } = default!;
}
