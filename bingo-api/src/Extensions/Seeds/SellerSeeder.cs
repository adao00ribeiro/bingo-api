using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Extensions.Seeds;

public class SellerSeeder : IDataSeeder
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly PaymentMethodSeeder _paymentSeeder;
    private readonly RoomSeeder _roomSeeder;

    public SellerSeeder(
        DataContext context,
        UserManager<User> userManager,
        PaymentMethodSeeder paymentSeeder,
        RoomSeeder roomSeeder)
    {
        _context = context;
        _userManager = userManager;
        _paymentSeeder = paymentSeeder;
        _roomSeeder = roomSeeder;
    }

    public async Task SeedAsync()
    {
        var sellerId = Guid.Parse("b9c2d2b5-eeae-486c-85ea-06dd5cfe0c06");
        var sellerEmail = "default@seller.com";

        // ----------------------------------------------------------------------
        // 1. SEED DO SELLER
        // ----------------------------------------------------------------------

        var existingSeller = await _context.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sellerId);

        if (existingSeller == null)
        {
            var newSeller = new Seller
            {
                Balance = 0,
                Email = sellerEmail,
                Cpf = "11111111111",
                DateBirth = DateTime.UtcNow,
                Comission = 0,
                IndicateRewardValue = 20
            };

            newSeller.SetIdGuid(sellerId);

            _context.Sellers.Add(newSeller);
            await _context.SaveChangesAsync();

            existingSeller = newSeller;
        }

        // ----------------------------------------------------------------------
        // 2. SEED PAYMENT METHODS
        // ----------------------------------------------------------------------

        await _paymentSeeder.SeedAsync(existingSeller.Id);

        // ----------------------------------------------------------------------
        // 3. SEED DEFAULT ROOMS
        // ----------------------------------------------------------------------

        await _roomSeeder.SeedAsync(existingSeller.Id);

        // ----------------------------------------------------------------------
        // 4. IDENTITY USER
        // ----------------------------------------------------------------------

        var existingUser = await _userManager.FindByIdAsync(sellerId.ToString());
        if (existingUser == null)
        {
            var identityUser = new User
            {
                Id = sellerId.ToString(),
                EntityId = sellerId,
                EntityType = nameof(Seller),
                UserName = sellerEmail,
                Email = sellerEmail,
                EmailConfirmed = true,
                PhoneNumber = "11111111111"
            };

            var createResult = await _userManager.CreateAsync(identityUser, "Admin@123");
            if (!createResult.Succeeded)
            {
                var errors = string.Join(" | ", createResult.Errors.Select(e => e.Description));
                throw new Exception($"Falha ao criar IdentityUser: {errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(" | ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Falha ao adicionar Role ao usuário: {errors}");
            }
        }
    }
}
