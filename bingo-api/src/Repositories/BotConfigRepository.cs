using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Bogus.Extensions.Brazil;

namespace bingo_api.src.Repositories;

public class BotConfigRepository : RepositoryBase<BotConfig>, IBotConfigRepository
{
    public BotConfigRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public async Task<BotConfig> UpdateAsync(Guid id , BotConfig objeto)
    {
        objeto.Id = id;
        await base.UpdateAsync(objeto);
        return objeto;
    }
    public async Task<BotConfig> CreateWithPuntersAsync(BotConfig botConfig)
    {
        using var transaction = await this.Context.Database.BeginTransactionAsync();

        try
        {
            await this.Context.BotConfigs.AddAsync(botConfig);
            await this.Context.SaveChangesAsync();

            var botConfigWithRelations = await Context.BotConfigs
                .Include(b => b.Room)
                    .ThenInclude(r => r.Owner)
                .FirstOrDefaultAsync(b => b.Id == botConfig.Id);

            if (botConfigWithRelations?.Room == null || botConfigWithRelations.Room.Owner == null)
                throw new Exception("Room ou Seller não encontrados!");

            var sellerId = botConfigWithRelations.Room.OwnerId;

            // Verifica se o Seller já tem Punters bots
            bool sellerHasPunters = await this.Context.Punters
                .AnyAsync(p => p.SellerId == sellerId&& p.IsBot == true);

            if (!sellerHasPunters) // Se não tem Punters, cria novos
            {
                var faker = new Faker("pt_BR");
                var nomesUsados = new HashSet<string>();
                var punters = new List<Punter>();

                for (int i = 0; i < 1000; i++)
                {
                    string nomeCompleto;
                    do
                    {
                        nomeCompleto = faker.Name.FullName();
                    } while (nomesUsados.Contains(nomeCompleto));
                  
                    nomesUsados.Add(nomeCompleto);
                    punters.Add(
                        new Punter(
                        faker.Internet.Email(nomeCompleto.ToLower().Replace(" ", ".")),
                    nomeCompleto,
                    faker.Person.Cpf().Replace(".", "").Replace("-", ""),
                    faker.Date.Past(60, DateTime.UtcNow.AddYears(-18)), sellerId,
                    true
                    ));
                }
                await this.Context.Punters.AddRangeAsync(punters);
                await this.Context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return botConfig;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<BotConfig> GetByRoomId(Guid roomId)
    {
          return await this.Context.BotConfigs
        .Include(b => b.Room)
        .FirstOrDefaultAsync(b => b.RoomId == roomId);
    }
}
