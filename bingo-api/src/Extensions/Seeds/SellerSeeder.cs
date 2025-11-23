using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity;

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

        if (_context.Sellers.Any(s => s.Id == sellerId)) return;

        var seller = new Seller
        {
            Balance = 0,
            Email = sellerEmail,
            Cpf = "11111111111",
            DateBirth = DateTime.UtcNow,
            Comission = 0,
            IndicateRewardValue = 20
        };
        seller.SetIdGuid(sellerId);

        var sellerAdded = _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        await _paymentSeeder.SeedForSellerAsync(seller.Id);
        await _roomSeeder.SeedForSellerAsync(seller.Id);

        var identityUser = new User
        {
            Id = sellerId.ToString(),
            EntityId = sellerAdded.Entity.Id,
            EntityType = nameof(Seller),
            UserName = sellerEmail,
            Email = sellerEmail,
            EmailConfirmed = true,
            PhoneNumber = "11111111111"
        };

        var result = _userManager.CreateAsync(identityUser, "Admin@123").Result;
        if (!result.Succeeded)
        {
            throw new Exception("Falha ao criar o usuário Identity para o Seller.");
        }
        var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
        if (!roleResult.Succeeded)
        {
            throw new Exception("Falha ao adicionar o Role ao usuário Seller.");
        }
    }
}
