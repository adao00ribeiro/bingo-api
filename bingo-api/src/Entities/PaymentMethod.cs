using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;

namespace bingo_api.src.Entities;

public class PaymentMethod : Entity
{
    public string Name { get; set; } // "Pix", "PushPay"
    public EPaymentMethodType Type { get; set; }
    public string? Token { get; set; } // Usado no PushPay
    public string? QrCodeUrl { get; set; } // Usado no Pix
    public string? Instructions { get; set; } // Texto adicional
    public bool Active { get; set; } = true;
    public Guid SellerId { get; set; }
    public Seller Seller { get; set; }

    public PaymentMethod(string name, EPaymentMethodType type, string? token, string? qrCodeUrl, string? instructions, bool active, Guid sellerId)
    {
        Name = name;
        Type = type;
        Token = token;
        QrCodeUrl = qrCodeUrl;
        Instructions = instructions;
        Active = active;
        SellerId = sellerId;
    }
}
