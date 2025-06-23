using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;
using bingo_api.src.Services;

namespace bingo_api.src.Entities;



//mudar para BalanceOperation 
public class Recharge : Entity
{

    public decimal Value { get; set; }
    public ERechargeStatus Status { get; set; } = ERechargeStatus.COMPLETED;
    public string Qrcode { get; set; }
    public string ImagemQrcode { get; set; }
    public Guid PunterId { get; set; }
    public Punter Punter { get; set; }


    public Recharge(decimal value, ERechargeStatus status, Guid punterId)
    {
        this.Value = value;
        this.Status = status;
        this.Qrcode = "";
        this.ImagemQrcode = "";
        this.PunterId = punterId;
    }

    public Recharge(QrCodeResponse qrCodeResponse,Guid punterId)
    {
        
       this.Id = qrCodeResponse.Id;
        this.Value = qrCodeResponse.Value/100;
        this.Status = ERechargeStatus.PENDING;
        this.Qrcode = qrCodeResponse.QrCode;
        this.ImagemQrcode = qrCodeResponse.QrCodeBase64;
        this.PunterId = punterId;
    }
}
