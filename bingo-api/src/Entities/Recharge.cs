using bingo_api.src.Adapter;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

namespace bingo_api.src.Entities;

public class Recharge : Entity
{
    // 🔹 Gateway
    public string? GatewayTransactionId { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    // 🔹 Crypto
    public string? Network { get; private set; }
    public string? Token { get; private set; }
    public string? DestinationAddress { get; private set; }
    public string? TxHash { get; private set; }

    // 🔹 PIX / QR
    public string? Qrcode { get; private set; }
    public string? ImagemQrcode { get; private set; }

    // 🔹 Controle
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CreditedAt { get; private set; }

    public decimal Value { get; private set; }
    public decimal Amount { get; private set; }

    public EPaymentStatus Status { get; private set; }

    public Guid PunterId { get; private set; }
    public Punter Punter { get; private set; } = null!;

    public bool IsConfirmed => Status == EPaymentStatus.CONFIRMED || Status == EPaymentStatus.CREDITED;
    public bool IsCredited => Status == EPaymentStatus.CREDITED;

    protected Recharge() { } // EF

    public Recharge(decimal value, decimal amount, Guid punterId)
    {
        Value = value;
        Amount = amount;
        PunterId = punterId;
        Status = EPaymentStatus.CREATED;
    }

    // =========================================================
    // 🔹 Gateway Initialization
    // =========================================================

    public void SetGatewayData(PaymentGatewayResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        GatewayTransactionId = result.GatewayTransactionId;
        Qrcode = result.QrCode;
        ImagemQrcode = result.QrImageUrl;
        DestinationAddress = result.WalletAddress;
        Network = result.Network;
        //Token = result.Token;
        ExpiresAt = result.ExpiresAt;

        Status = EPaymentStatus.WAITING_PAYMENT;
    }

    // =========================================================
    // 🔹 Webhook → Confirma pagamento no gateway
    // =========================================================

    public void ConfirmFromGateway(string? txHash = null)
    {
        if (Status == EPaymentStatus.CREDITED)
            return; // idempotência real

        if (Status == EPaymentStatus.CONFIRMED)
            return;

        if (Status is EPaymentStatus.FAILED or 
            EPaymentStatus.CANCELED or 
            EPaymentStatus.EXPIRED)
            throw new InvalidOperationException("Recharge cannot be confirmed.");

        TxHash = txHash;
        ConfirmedAt = DateTime.UtcNow;
        Status = EPaymentStatus.CONFIRMED;
    }

    // =========================================================
    // 🔹 Crédito interno (após criar TransactionHistory)
    // =========================================================

    public void MarkAsCredited()
    {
        if (Status != EPaymentStatus.CONFIRMED)
            throw new InvalidOperationException("Recharge must be confirmed before crediting.");

        CreditedAt = DateTime.UtcNow;
        Status = EPaymentStatus.CREDITED;
    }

    // =========================================================
    // 🔹 Falhas
    // =========================================================

    public void MarkAsFailed()
    {
        if (IsCredited)
            throw new InvalidOperationException("Cannot fail a credited recharge.");

        Status = EPaymentStatus.FAILED;
    }

    public void MarkAsExpired()
    {
        if (IsCredited)
            return;

        Status = EPaymentStatus.EXPIRED;
    }

    public void Cancel()
    {
        if (IsCredited)
            throw new InvalidOperationException("Cannot cancel a credited recharge.");

        Status = EPaymentStatus.CANCELED;
    }

    // =========================================================
    // 🔹 Map Gateway Status
    // =========================================================

    public static EPaymentStatus MapStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return EPaymentStatus.WAITING_PAYMENT;

        status = status.ToLowerInvariant();

        return status switch
        {
            "paid" or "completed" or "approved" or "aprovado"
                => EPaymentStatus.CONFIRMED,

            "failed"
                => EPaymentStatus.FAILED,

            "canceled"
                => EPaymentStatus.CANCELED,

            "expired"
                => EPaymentStatus.EXPIRED,

            "pending"
                => EPaymentStatus.WAITING_PAYMENT,

            _ => EPaymentStatus.WAITING_PAYMENT
        };
    }
}
