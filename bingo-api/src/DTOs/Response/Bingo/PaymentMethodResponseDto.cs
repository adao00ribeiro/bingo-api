using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.DTOs.Response.Bingo;

public record PaymentMethodResponseDto
{
    public Guid Id { get; set; } // "Pix", "PushPay"
    public string Name { get; set; } // "Pix", "PushPay"
    public EPaymentMethodType Type { get; set; }
    public string? PixPayload { get; set; } // Copia e Cola
    public string? QrCodeUrl { get; set; } // Usado no Pix
    public string? Instructions { get; set; } // Texto adicional
    public bool Active { get; set; } 
    public Guid OnlineHouseId { get; set; }

 
    internal static object ConvertToDto(object room)
    {
        throw new NotImplementedException();
    }

    internal static PaymentMethodResponseDto ConvertToDtoToOnlineHouse(PaymentMethod r)
    {
      return new PaymentMethodResponseDto
      {
          Id = r.Id,
          Name = r.Name,
          Type = r.Type,
          PixPayload = r.PixPayload,
          QrCodeUrl = r.QrCodeUrl,
          Instructions = r.Instructions,
          Active = r.Active
         
      };
    }
}
