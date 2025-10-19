using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Enums;

namespace bingo_api.src.Extensions.Seeds
{
    public class ScratchGameSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public ScratchGameSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.ScratchGames.Any())
                return;

            // 🐄 Fortuna 3x3
            var fortuna = new ScratchGame
            {
                Name = "Fortuna 3x3",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 5.00m,
                MaxPrize = 500_000m,
                Probability = 3.1m,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 5.00m),
                        new(2, 10.00m),
                        new(5, 25.00m),
                        new(10, 50.00m),
                        new(50, 250.00m),
                        new(100, 500.00m),
                        new(500, 2_500.00m),
                        new(1_000, 5_000.00m),
                        new(10_000, 50_000.00m),
                        new(100_000, 500_000.00m),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🐄", Name = "Vaca Dourada", PrizeValue = 5.00m, Weight = 100 },
                        new() { Symbol = "🦝", Name = "Guaxinim Ninja", PrizeValue = 10.00m, Weight = 80 },
                        new() { Symbol = "🐨", Name = "Coala Zen", PrizeValue = 25.00m, Weight = 60 },
                        new() { Symbol = "🦘", Name = "Canguru Boxeador", PrizeValue = 50.00m, Weight = 40 },
                        new() { Symbol = "🦓", Name = "Zebra Listrada", PrizeValue = 250.00m, Weight = 25 },
                        new() { Symbol = "🐵", Name = "Macaco Sábio", PrizeValue = 500.00m, Weight = 15 },
                        new() { Symbol = "🦏", Name = "Rinoceronte Blindado", PrizeValue = 2_500.00m, Weight = 8 },
                        new() { Symbol = "🐘", Name = "Elefante Real", PrizeValue = 5_000.00m, Weight = 4 },
                        new() { Symbol = "🦁", Name = "Leão Dourado", PrizeValue = 50_000.00m, Weight = 2 },
                        new() { Symbol = "🐯", Name = "Tigre Feroz", PrizeValue = 500_000.00m, Weight = 1 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 🍀 Trevo da Sorte
            var trevo = new ScratchGame
            {
                Name = "Trevo da Sorte",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00m),
                        new(2, 20.00m),
                        new(5, 50.00m),
                        new(10, 100.00m),
                        new(50, 500.00m),
                        new(100, 1_000.00m),
                        new(500, 5_000.00m),
                        new(1_000, 10_000.00m),
                        new(10_000, 100_000.00m),
                        new(100_000, 1_000_000.00m),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00m, Weight = 100 },
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00m, Weight = 80 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00m, Weight = 60 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00m, Weight = 40 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00m, Weight = 25 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00m, Weight = 15 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00m, Weight = 8 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00m, Weight = 4 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00m, Weight = 2 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00m, Weight = 1 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ScratchGames.AddRange(fortuna, trevo);
            await _context.SaveChangesAsync();
        }
    }

    // Record simples, usado no PayoutTable
    public record ScratchPayout(decimal Multiplier, decimal Prize);
}
