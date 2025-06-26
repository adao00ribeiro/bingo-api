using System.Transactions;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories;

public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
{
    public RechargeRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public override async Task<Recharge?> GetByIdAsync(Guid id)
    {
            var recharge = await Context.Recharges
             .Include(r => r.Punter)
                 .ThenInclude(p => p.Seller)
            .FirstOrDefaultAsync(recharge => recharge.Id == id);
        return recharge;
    }
    public async Task<bool> UpdateStatusToCompleted(Guid id, Seller seller)
    {
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var recharge = await Context.Recharges.FindAsync(id);

                if (recharge is null)
                {
                    throw new Exception("Recharge nao encontrado.");
                }
                if (recharge.Status == ERechargeStatus.COMPLETED )
                {
                     throw new Exception("Esta recarga já foi concluída anteriormente.");
                }
                // Atualiza o status para COMPLETED
                recharge.Status = ERechargeStatus.COMPLETED;
                await Context.SaveChangesAsync();

                var punter = await Context.Punters.FindAsync(recharge.PunterId);
                if (punter == null)
                {
                    throw new Exception("Punter nao encontrado.");
                }

                if (punter.Balance == 0 && punter.IndicateTag is not null)
                {
                    var indicatePunter = await Context.Punters.FirstAsync(x => x.IndicateTag == punter.IndicateTag);

                    if (indicatePunter != null)
                    {
                        var bonusIndicate = seller.IndicateRewardValue;
                        var transactionIndicateHistory = new TransactionHistory
                        {
                            EntityType = "Punter",
                            EntityId = indicatePunter.Id,
                            PreviousBalance = indicatePunter.Balance,
                            CurrentBalance = indicatePunter.Balance + bonusIndicate,
                            Amount = bonusIndicate,
                            Type = TransactionType.Reward,
                        };

                        await this.Context.TransactionHistories.AddAsync(transactionIndicateHistory);
                        indicatePunter.Balance += bonusIndicate;
                    }
                }

                var transactionHistory = new TransactionHistory
                {
                    EntityType = "Punter",
                    EntityId = punter.Id,
                    PreviousBalance = punter.Balance,
                    CurrentBalance = punter.Balance + recharge.Value,
                    Amount = recharge.Value,
                    Type = TransactionType.Deposit,
                };

                await this.Context.TransactionHistories.AddAsync(transactionHistory);


                punter.Balance += recharge.Value;
                await Context.SaveChangesAsync();
                transaction.Complete();
                return true;
            }
            catch (Exception ex)
            {
                // Loga o erro (aqui você pode usar uma ferramenta de logging como Serilog, NLog ou log para um arquivo)
                Console.Error.WriteLine($"Erro ao atualizar o status para COMPLETED: {ex.Message}");

                // Pode lançar novamente ou retornar um valor indicando que houve erro
                return false;
            }
        }
    }

    public async Task<int> CountAsync(Guid punterId)
    {
        return await Context.Recharges.CountAsync(r => r.PunterId == punterId);
    }
}
