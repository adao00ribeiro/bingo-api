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

            //
            //  JOGO: BICHO MANIA
            //
            var bixo = new ScratchGame
            {
                Name = "Bicho Mania",
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
                        new(0.50, 10),
                        new(0.30, 25),
                        new(0.01, 50),
                        new(0.05, 250),
                        new(0.01, 500),
                        new(0.007, 2_500),
                        new(0.004, 5_000),
                        new(0.002, 50_000),
                        new(0.001, 500_000),
                        new(0.0001, 5_000_000)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🐄", Name = "Vaca Dourada", PrizeValue = 5 },
                        new() { Symbol = "🦝", Name = "Guaxinim Ninja", PrizeValue = 10 },
                        new() { Symbol = "🐨", Name = "Coala Zen", PrizeValue = 25 },
                        new() { Symbol = "🦘", Name = "Canguru Boxeador", PrizeValue = 50 },
                        new() { Symbol = "🦓", Name = "Zebra Listrada", PrizeValue = 250 },
                        new() { Symbol = "🐵", Name = "Macaco Sábio", PrizeValue = 500 },
                        new() { Symbol = "🦏", Name = "Rinoceronte Blindado", PrizeValue = 2_500 },
                        new() { Symbol = "🐘", Name = "Elefante Real", PrizeValue = 5_000 },
                        new() { Symbol = "🦁", Name = "Leão Dourado", PrizeValue = 50_000 },
                        new() { Symbol = "🐯", Name = "Tigre Feroz", PrizeValue = 500_000 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: TREVO DA SORTE
            //
            var trevo = new ScratchGame
            {
                Name = "Trevo da Sorte",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 0.75m,
                MaxPrize = 7_500,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },

                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(0.80, 0.75),
                        new(0.60, 1.50),
                        new(0.40, 3.00),
                        new(0.25, 3.75),
                        new(0.10, 7.50),
                        new(0.05, 15.00),
                        new(0.01, 375.00),
                        new(0.005, 750.00),
                        new(0.002, 2_250.00),
                        new(0.0005, 7_500.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 0.75 },
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 1.50 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 3.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 3.75 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 7.50 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 15.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 375.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 750.00 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 2_250.00 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 7_500.00 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: 777
            //
            var sete = new ScratchGame
            {
                Name = "777",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 5.00m,
                MaxPrize = 77_770,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },

                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(0.80, 2.50),
                        new(0.60, 10.00),
                        new(0.40, 50.00),
                        new(0.20, 500.00),
                        new(0.10, 1_000.00),
                        new(0.01, 5_000.00),
                        new(0.005, 10_000.00),
                        new(0.0005, 77_770.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 2.50 },
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 10.00 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50.00 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 500.00 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 1_000.00 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 5_000.00 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 10_000.00 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 77_770.00 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: MILIONARIO INSTANTANEO
            //
            var milionarioinstantaneo = new ScratchGame
            {
                Name = "MilionarioInstantaneo",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price = 12.00m,
                MaxPrize = 3_000_000,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },

                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(0.80, 18.00),
                        new(0.60, 24.00),
                        new(0.50, 30.00),
                        new(0.30, 60.00),
                        new(0.15, 120.00),
                        new(0.05, 600.00),
                        new(0.01, 3_000.00),
                        new(0.005, 30_000.00),
                        new(0.001, 300_000.00),
                        new(0.0001, 3_000_000.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 18 },
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 24 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 30 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 60 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 120 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 600 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 3_000 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 30_000 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 300_000 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 3_000_000 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: OURO
            //
            var ouro = new ScratchGame
            {
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
                        new(0.80, 10.00),
                        new(0.60, 20.00),
                        new(0.40, 50.00),
                        new(0.25, 100.00),
                        new(0.10, 500.00),
                        new(0.05, 1_000.00),
                        new(0.01, 5_000.00),
                        new(0.005, 10_000.00),
                        new(0.001, 100_000.00),
                        new(0.0001, 1_000_000.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🍀", Name = "Trevo da Sorte", PrizeValue = 10 },
                        new() { Symbol = "💎", Name = "Diamante", PrizeValue = 20 },
                        new() { Symbol = "🔔", Name = "Sino da Fortuna", PrizeValue = 50 },
                        new() { Symbol = "🍒", Name = "Cereja", PrizeValue = 100 },
                        new() { Symbol = "💰", Name = "Saco de Dinheiro", PrizeValue = 500 },
                        new() { Symbol = "👑", Name = "Coroa Real", PrizeValue = 1_000 },
                        new() { Symbol = "🔥", Name = "Chama Dourada", PrizeValue = 5_000 },
                        new() { Symbol = "🌈", Name = "Arco-Íris da Fortuna", PrizeValue = 10_000 },
                        new() { Symbol = "⭐", Name = "Estrela Suprema", PrizeValue = 100_000 },
                        new() { Symbol = "👼", Name = "Anjo da Sorte", PrizeValue = 1_000_000 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: COFRINHO
            //
            var cofrinho = new ScratchGame
            {
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
                        new(0.80, 0.30),
                        new(0.50, 0.90),
                        new(0.30, 1.50),
                        new(0.01, 3.00),
                        new(0.05, 4.50),
                        new(0.01, 15.00),
                        new(0.007, 150.00),
                        new(0.004, 450.00),
                        new(0.002, 1_500.00),
                        new(0.001, 7_500.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "🐷", Name = "Porquinho da Sorte", PrizeValue = 0.30 },
                        new() { Symbol = "🥉", Name = "Moeda de Bronze", PrizeValue = 0.90 },
                        new() { Symbol = "💰", Name = "Bolsa de Moedas", PrizeValue = 1.50 },
                        new() { Symbol = "💵", Name = "Cédula da Sorte", PrizeValue = 3.00 },
                        new() { Symbol = "🏦", Name = "Mini Cofre", PrizeValue = 4.50 },
                        new() { Symbol = "💎", Name = "Diamante Pequeno", PrizeValue = 15.00 },
                        new() { Symbol = "🔥", Name = "Tocha Dourada", PrizeValue = 150.00 },
                        new() { Symbol = "🌟", Name = "Estrela Forte", PrizeValue = 450.00 },
                        new() { Symbol = "👑", Name = "Coroa Cofrinho", PrizeValue = 1_500.00 },
                        new() { Symbol = "🏆", Name = "Grande Tesouro", PrizeValue = 7_500.00 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  JOGO: CRYPTOMOEDA
            //
            var cryptomoeda = new ScratchGame
            {
                Name = "CryptoFortune",
                LayoutType = EScratchLayoutType.Layout3x3,
                Price =0.90m,
                MaxPrize = 1_000_000m,
                Probability = 2.8m,
                AllowedMultipliers = new[] { 1, 2, 5, 10, 50 },

                Attributes = new ScratchGameAttributes
                {
                    PayoutTable = new List<ScratchPayout>
                    {
                        new(0.40, 0.30),
                        new(0.20, 0.90),
                        new(0.15, 1.50),
                        new(0.10, 3.00),
                        new(0.06, 4.50),
                        new(0.04, 15.00),
                        new(0.015, 150.00),
                        new(0.008, 450.00),
                        new(0.002, 1_500.00),
                        new(0.0002, 5_000.00)
                    },

                    Symbols = new List<ScratchSymbol>
                    {
                        new() { Symbol = "₿",  Name = "Bitcoin", PrizeValue = 0.30 },
                        new() { Symbol = "Ξ",  Name = "Ethereum", PrizeValue = 0.90 },
                        new() { Symbol = "Ł",  Name = "Litecoin", PrizeValue = 1.50 },
                        new() { Symbol = "ⓧ", Name = "XRP", PrizeValue = 3.00 },
                        new() { Symbol = "Ð",  Name = "Dogecoin", PrizeValue = 4.50 },
                        new() { Symbol = "ⓢ", Name = "Solana", PrizeValue = 15.00 },
                        new() { Symbol = "Ⓣ", Name = "Tron", PrizeValue = 150.00 },
                        new() { Symbol = "Ⓛ", Name = "LINK (Chainlink)", PrizeValue = 450.00 },
                        new() { Symbol = "Ⓜ", Name = "Monero", PrizeValue = 1_500.00 },
                        new() { Symbol = "₿+", Name = "Bitcoin Black Premium", PrizeValue = 5_000.00 }
                    }
                },

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            //
            //  SALVAR TODOS
            //
            _context.ScratchGames.AddRange(
                bixo,
                trevo,
                sete,
                milionarioinstantaneo,
                ouro,
                cofrinho,
                cryptomoeda
            );

            await _context.SaveChangesAsync();
        }
    }

    public record ScratchPayout(double Probability, double Prize);
}
