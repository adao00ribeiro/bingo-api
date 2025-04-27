using bingo_api.src.Constants;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace bingo_api.src.Extensions;

public class DataInitializer
{

    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IBotConfigRepository _botConfigRepository;
    public DataInitializer(
       DataContext context,
       UserManager<User> userManager,
       RoleManager<IdentityRole> roleManager,
       IBotConfigRepository botConfigRepository)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _botConfigRepository = botConfigRepository;
    }
    public async Task Seed()
    {

        var roles = new[] { Roles.Admin, Roles.Seller, Roles.Punter };

        foreach (var role in roles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        var sellerId = Guid.Parse("b9c2d2b5-eeae-486c-85ea-06dd5cfe0c06");
        var sellerEmail = "default@seller.com";

        if (!_context.Sellers.Any(s => s.Id == sellerId))
        {


            // Cria um Seller de desenvolvimento
            var seller = new Seller
            {
                Balance = 0,
                Email = sellerEmail,
                Cpf = "11111111111",
                DateBirth = DateTime.UtcNow,
                Comission = 0
            };
            seller.SetIdGuid(sellerId);
            // Adiciona o Seller ao contexto
            var sellerAdded = _context.Sellers.Add(seller);

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

            // Cria uma Room associada ao Seller
            var room = new Room("Sala de Desenvolvimento", sellerId);

            room.Accumulated = new Accumulated
            {
                Activated = true,
                MinimumValue = 50,
                MaximumValue = 5000,
                CurrentValue = 100,
                MaximumNumberOfBalls = 45,
                CumulativePercentage = 2.5m,
                IncrementBallCumulative = true,
                RoomId = room.Id
            };
           
            _context.Rooms.Add(room);
            _context.SaveChanges(); // Salva a Room no banco de dados

           await this._botConfigRepository.CreateWithPuntersAsync(new BotConfig(room.Id));

        }
    }
}