using bingo_api.src.Context;
using bingo_api.src.Entities.Scratch;
using bingo_api.src.Enums;
using bingo_api.src.Structs.Scratchcard;

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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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
                QuantityToAward = 3,
                AllowedMultipliers = new[] { 1, 5, 10, 25 },
                Rows = 3,
                Cols = 3,
                PayoutTable = new List<ScratchPayout>

                    {
                        new(0, 0.667389),
                        new(0.4, 0.24),
                        new(1.2, 0.06),
                        new(2,  0.02),
                        new(4, 0.01),
                        new(6,  0.002),
                        new(20,  0.0005),
                        new(200, 0.0001),
                        new(600, 0.00002),
                        new(2000, 0.000001),
                        new(10000, 1e-7)
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

}
