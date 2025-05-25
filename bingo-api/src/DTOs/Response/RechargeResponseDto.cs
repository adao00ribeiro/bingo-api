


using System.Text.Json.Serialization;
using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;
using bingo_api.src.Enums;


namespace bingo_api.src.DTOs.Response;

public record RechargeResponseDto : EntityResponseDto
{
    public decimal Value { get; set; }
    public ERechargeStatus Status { get; set; } = ERechargeStatus.PENDING;
    public string Qrcode { get; set; }
    public string ImagemQrcode { get; set; }
    public Guid PunterId { get; set; }
    public PunterResponseDto? Punter { get; set; }

    public RechargeResponseDto(
        Guid id,
        decimal value,
        ERechargeStatus status,
        string qrcode,
        string imagemQrcode,
        Guid punterId,
        PunterResponseDto? punter,
        DateTime createAt,
        DateTime updateAt
        ) : base(id, createAt, updateAt)
    {
        Id = id;
        Value = value;
        Status = status;
        Qrcode = qrcode;
        ImagemQrcode = imagemQrcode;
        PunterId = punterId;
        Punter = punter;
        CreateAt = createAt;
        UpdateAt = updateAt;
    }

    internal static RechargeResponseDto ConvertToDto(Recharge recharge)
    {
        var punterResponse = recharge.Punter != null ? PunterResponseDto.ConvertToDto(recharge.Punter) : null;
        return new RechargeResponseDto(
            recharge.Id,
            recharge.Value,
            recharge.Status,
            recharge.Qrcode,
            recharge.ImagemQrcode,
            recharge.PunterId,
            punterResponse,
            recharge.CreateAt,
            recharge.UpdateAt
        );
    }
}
