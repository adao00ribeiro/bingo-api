using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
