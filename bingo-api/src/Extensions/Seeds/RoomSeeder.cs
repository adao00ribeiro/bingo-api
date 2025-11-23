using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;

namespace bingo_api.src.Extensions.Seeds;

public class RoomSeeder
{
    private readonly DataContext _context;
    private readonly IBotConfigRepository _botConfigRepository;

    public RoomSeeder(DataContext context, IBotConfigRepository botConfigRepository)
    {
        _context = context;
        _botConfigRepository = botConfigRepository;
    }

    public async Task SeedAsync(Guid sellerId)
    {
        var room = new Room("Sala de Desenvolvimento", sellerId)
        {
            Accumulated = new Accumulated
            {
                Activated = true,
                MinimumValue = 50,
                MaximumValue = 5000,
                CurrentValue = 100,
                MaximumNumberOfBalls = 45,
                CumulativePercentage = 2.5m,
                IncrementBallCumulative = true,
            }
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        await _botConfigRepository.CreateWithPuntersAsync(new BotConfig(room));
    }
}
