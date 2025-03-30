using System.Transactions;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories;

public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
{
    public RechargeRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<bool> UpdateStatusToCompleted(Guid id)
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

                // Atualiza o status para COMPLETED
                recharge.Status = ERechargeStatus.COMPLETED;
                await Context.SaveChangesAsync();

                var punter = await Context.Punters.FindAsync(recharge.PunterId);
                if (punter == null)
                {
                    throw new Exception("Punter nao encontrado.");
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
}
