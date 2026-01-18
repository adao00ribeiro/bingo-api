using bingo_api.src.Domain.Events;
using bingo_api.src.Entities.Bingo;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

namespace bingo_api.src.Entities;

public class PaymentMethod : Entity
{
    public string Name { get; set; } // "Pix", "PushPay"
    public EPaymentMethodType Type { get; set; }
    public string? Token { get; set; } // Usado no PushPay
    public string? PixPayload { get; set; } // Copia e Cola
    public string? QrCodeUrl { get; set; } // Usado no Pix
    public string? Instructions { get; set; } // Texto adicional
    public bool Active { get; set; } = true;
    public Guid OnlineHouseId { get; set; }
    public OnlineHouse OnlineHouse { get; set; }

    public PaymentMethod(string name, EPaymentMethodType type, string? token, string? pixPayload, string? qrCodeUrl, string? instructions, bool active, Guid onlineHouseId)
    {
        Name = name;
        Type = type;
        Token = token;
        PixPayload = pixPayload;
        QrCodeUrl = qrCodeUrl;
        Instructions = instructions;
        Active = active;
        OnlineHouseId = onlineHouseId;
        AddDomainEvent(new PixPayloadUpdatedEvent(this));
    }

}
