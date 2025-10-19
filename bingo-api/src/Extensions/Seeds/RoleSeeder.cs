using bingo_api.src.Constants;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.Extensions.Seeds;

public class RoleSeeder : IDataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        var roles = new[] { Roles.Admin, Roles.Seller, Roles.Punter };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
