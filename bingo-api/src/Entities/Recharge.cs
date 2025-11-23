using bingo_api.src.Adapter;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

namespace bingo_api.src.Entities;



//mudar para BalanceOperation 
public class Recharge : Entity
{
    public string? Network { get; set; }  // "Ethereum", "Bsc", etc.
    public string? Token { get; set; }  // "USDT", "USDC", etc.
    public string? DestinationAddress { get; set; }  // address where the user should send the tokens
    public string? TxHash { get; set; } // transaction hash sent by the user
    public DateTime? ConfirmedAt { get; set; } // date/time it was confirmed
    public decimal Value { get; set; }
    public decimal Amount { get; set; }
    public ERechargeStatus Status { get; set; } = ERechargeStatus.COMPLETED;
    public string Qrcode { get; set; }
    public string ImagemQrcode { get; set; }
    public Guid PunterId { get; set; }
    public Punter Punter { get; set; }
    public bool IsConfirmed => ConfirmedAt.HasValue; // calculated property


    public Recharge(decimal value, decimal amount , ERechargeStatus status, Guid punterId)
    {
        this.Value = value;
        this.Amount = amount;
        this.Status = status;
        this.Qrcode = "";
        this.ImagemQrcode = "";
        this.PunterId = punterId;
    }

    public Recharge(QrCodeResponse qrCodeResponse, Guid punterId)
    {

        this.Id = qrCodeResponse.Id;
        this.Value = qrCodeResponse.Value / 100;
        this.Status = ERechargeStatus.PENDING;
        this.Qrcode = qrCodeResponse.QrCode;
        this.ImagemQrcode = qrCodeResponse.QrCodeBase64;
        this.PunterId = punterId;
    }
}
