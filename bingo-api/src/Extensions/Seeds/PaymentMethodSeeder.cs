using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;

namespace bingo_api.src.Extensions.Seeds;

public class PaymentMethodSeeder
{
    private readonly DataContext _context;

    public PaymentMethodSeeder(DataContext context)
    {
        _context = context;
    }

    public async Task SeedForSellerAsync(Guid sellerId)
    {
        if (_context.PaymentMethods.Any(pm => pm.SellerId == sellerId))
            return;

        var pixManual = new PaymentMethod(
            "PIX Manual",
            EPaymentMethodType.PIXMANUAL,
            "",
            "https://exemplo.com/qrcode.png",
            "Escaneie o QR Code e envie o comprovante para o suporte.",
            true,
            sellerId
        );

        var pushPay = new PaymentMethod(
            "PushPay",
            EPaymentMethodType.PUSHPAY,
            "SEU_TOKEN_PADRAO_SE_FOR_APLICÁVEL",
            "",
            "",
            false,
            sellerId
        );

        _context.PaymentMethods.AddRange(pixManual, pushPay);
        await _context.SaveChangesAsync();
    }
}
