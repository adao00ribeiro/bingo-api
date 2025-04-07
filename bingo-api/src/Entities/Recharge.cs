using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

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

}
