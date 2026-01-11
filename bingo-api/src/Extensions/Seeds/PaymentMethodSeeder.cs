using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Extensions.Seeds;

public class PaymentMethodSeeder
{
    private readonly DataContext _context;

    public PaymentMethodSeeder(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(Guid sellerId)
    {
        // Define todos os métodos que devem existir
        var requiredMethods = new List<PaymentMethod>
        {
            new PaymentMethod(
                "PIX Manual",
                EPaymentMethodType.PIXMANUAL,
                "",
                "https://exemplo.com/qrcode.png",
                "Escaneie o QR Code e envie o comprovante para o suporte.",
                true,
                sellerId
            ),

            new PaymentMethod(
                "PushPay",
                EPaymentMethodType.PUSHPAY,
                "SEU_TOKEN_PADRAO_SE_FOR_APLICÁVEL",
                "",
                "",
                false,
                sellerId
            ),

            new PaymentMethod(
                "Crypto",
                EPaymentMethodType.CRYPTO,
                "SEU_TOKEN_PADRAO_SE_FOR_APLICÁVEL",
                "",
                "",
                false,
                sellerId
            )
        };

        // Busca do banco somente os tipos já existentes
        var existingTypes = await _context.PaymentMethods
            .Where(pm => pm.OnlineHouseId == sellerId)
            .Select(pm => pm.Type)
            .ToListAsync();

        // Filtra apenas os que não existem ainda
        var toInsert = requiredMethods
            .Where(rm => !existingTypes.Contains(rm.Type))
            .ToList();

        if (toInsert.Any())
        {
            _context.PaymentMethods.AddRange(toInsert);
            await _context.SaveChangesAsync();
        }
    }
}
