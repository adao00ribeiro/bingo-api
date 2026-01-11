using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task SeedAsync(Guid OnlineHouseId)
    {
        const string defaultRoomName = "Sala de Desenvolvimento";
        // ------------------------------------------------------------
        // 1. Verifica se a sala já existe
        // ------------------------------------------------------------
        var existingRoom = await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.OwnerId == OnlineHouseId && r.Name == defaultRoomName);

        if (existingRoom != null)
            return; // idempotência garantida

        // ------------------------------------------------------------
        // 2. Criar a nova sala
        // ------------------------------------------------------------
        var room = new Room(defaultRoomName, OnlineHouseId)
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

        // ------------------------------------------------------------
        // 3. Criar BotConfig apenas após a sala ser criada
        // ------------------------------------------------------------
        await _botConfigRepository.CreateWithPuntersAsync(new BotConfig(room));
    }
}
