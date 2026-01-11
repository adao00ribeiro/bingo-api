using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Entities.Bingo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Extensions.Seeds;

public class SellerSeeder : IDataSeeder
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly PaymentMethodSeeder _paymentSeeder;
    private readonly RoomSeeder _roomSeeder;
    private const string SellerEmail = "default@seller.com";

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
        // ----------------------------------------------------------------------
        // 1. SEED DO SELLER
        // ----------------------------------------------------------------------

        var seller = await _context.Sellers
          .FirstOrDefaultAsync(s => s.Email == SellerEmail);

        if (seller == null)
        {
           seller = new Seller
            {
                Balance = 0,
                Email = SellerEmail,
                Cpf = "11111111111",
                DateBirth = DateTime.UtcNow,
                Comission = 0,
                IndicateRewardValue = 20
            };

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
        }

        var onlineHouse = await _context.OnlineHouses
            .FirstOrDefaultAsync(o => o.SellerId == seller.Id);

        if (onlineHouse == null)
        {
            onlineHouse = new OnlineHouse(
                name: "Demonstrativo",
                sellerId: seller.Id
            )
            {
                Hostname = "localhost"
            };

            _context.OnlineHouses.Add(onlineHouse);
            await _context.SaveChangesAsync();
        }
        // ----------------------------------------------------------------------
        // 2. SEED PAYMENT METHODS
        // ----------------------------------------------------------------------
        await _paymentSeeder.SeedAsync(onlineHouse.Id);
        // ----------------------------------------------------------------------
        // 3. SEED DEFAULT ROOMS
        // ----------------------------------------------------------------------

        await _roomSeeder.SeedAsync(onlineHouse.Id);

        // ----------------------------------------------------------------------
        // 4. IDENTITY USER
        // ----------------------------------------------------------------------

        var existingUser = await _userManager.FindByEmailAsync(SellerEmail);
        if (existingUser == null)
        {
            var identityUser = new User
            {
                EntityId = seller.Id,
                EntityType = nameof(Seller),
                UserName = SellerEmail,
                Email = SellerEmail,
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
