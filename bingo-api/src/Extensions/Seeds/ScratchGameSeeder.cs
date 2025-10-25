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
                    new(0.80, 5),
                    new(0.50, 10.00),
                    new(0.30, 25.00),
                    new(0.01, 50.00),
                    new(0.05, 250.00),
                    new(0.01, 500.00),
                    new(0.007, 2_500.00),
                    new(0.004, 5_000.00),
                    new(0.002, 50_000.00),
                    new(0.001, 500_000.00),
                    new(0.0001, 5_000_000.00)
                },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🐄", Name = "Vaca Dourada", PrizeValue = 0},
                        new() { Symbol = "🦝", Name = "Guaxinim Ninja", PrizeValue = 10.00},
                        new() { Symbol = "🐨", Name = "Coala Zen", PrizeValue = 25.00},
                        new() { Symbol = "🦘", Name = "Canguru Boxeador", PrizeValue = 50.00},
                        new() { Symbol = "🦓", Name = "Zebra Listrada", PrizeValue = 250.00 },
                        new() { Symbol = "🐵", Name = "Macaco Sábio", PrizeValue = 500.00},
                        new() { Symbol = "🦏", Name = "Rinoceronte Blindado", PrizeValue = 2_500.00 },
                        new() { Symbol = "🐘", Name = "Elefante Real", PrizeValue = 5_000.00 },
                        new() { Symbol = "🦁", Name = "Leão Dourado", PrizeValue = 50_000.00 },
                        new() { Symbol = "🐯", Name = "Tigre Feroz", PrizeValue = 500_000.00 }                    }
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
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var sete = new ScratchGame
            {
                Name = "777",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var ouro = new ScratchGame{
                Name = "Ouro",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var cofrinho = new ScratchGame{
                Name = "Cofrinho",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var frutinha = new ScratchGame{
                Name = "Frutinha",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var cryptomoeda = new ScratchGame{
                Name = "Cryptomoeda",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
                    }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var milionarioinstantaneo =new ScratchGame{
                Name = "MilionarioInstantaneo",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 10.00m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },
                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(1, 10.00),
                        new(2, 20.00),
                        new(5, 50.00),
                        new(10, 100.00),
                        new(50, 500.00),
                        new(100, 1_000.00),
                        new(500, 5_000.00),
                        new(1_000, 10_000.00),
                        new(10_000, 100_000.00),
                        new(100_000, 1_000_000.00),
                    },
                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10.00},
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000.00 },
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
    public record ScratchPayout(double probability, double Prize);
}
