using System.Transactions;
using bingo_api.src.Context;
using bingo_api.src.Entities;
using bingo_api.src.Enums;
using bingo_api.src.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly DataContext _context;
    private readonly TelegamNotifierService _telegamNotifierService;

    public WithdrawalService(DataContext context, TelegamNotifierService telegamNotifierService)
    {
        _context = context;
        _telegamNotifierService = telegamNotifierService;
    }

    public async Task<(bool Success, string Message)> CreateWithdrawalAsync(Guid entityId, decimal amount)
    {
        // Tenta buscar como Punter
        var punter = await _context.Punters.FirstOrDefaultAsync(p => p.Id == entityId);
        if (punter != null)
        {
            if (punter.PrizeBalance < amount)
                return (false, "Saldo insuficiente.");

            var withdrawal = new PunterWithdrawal
            {
                PunterId = punter.Id,
                Amount = amount,
                Status = EPaymentStatus.PENDING
            };

            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            await _telegamNotifierService.SendMessageAsync(
    $"⚠️ Pedido de saque do CPF {punter.Cpf} no valor de R$ {amount.ToString("N2")}");
            return (true, "Saque do Punter registrado com sucesso.");
        }

        // Tenta buscar como Seller
        var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.Id == entityId);
        if (seller != null)
        {
            if (seller.Balance < amount)
                return (false, "Saldo insuficiente.");

            var withdrawal = new SellerWithdrawal
            {
                SellerId = seller.Id,
                Amount = amount,
                Status = EPaymentStatus.PENDING
            };

            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            return (true, "Saque do Seller registrado com sucesso.");
        }

        return (false, "Entidade não encontrada ou tipo inválido.");
    }

    public async Task<bool> UpdateStatusToCompleted(Guid id, bool isAdmin, Guid? sellerId)
    {
        Console.WriteLine("aki" + id);
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            // Carrega o Withdrawal (tanto Punter quanto Seller)
            var withdrawal = await _context.Withdrawals
                .Include(w => (w as PunterWithdrawal).Punter)
                .Include(w => (w as SellerWithdrawal).Seller)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (withdrawal is null)
                throw new InvalidOperationException("Saque não encontrado.");

            if (withdrawal.Status == EPaymentStatus.SUCCESS)
                throw new InvalidOperationException("Este saque já foi concluído anteriormente.");


            // ======================================================================================
            // 1) PERMISSÕES
            // ======================================================================================
            if (!isAdmin)
            {
                if (!sellerId.HasValue)
                    throw new UnauthorizedAccessException("Acesso não permitido.");

                switch (withdrawal)
                {
                    case PunterWithdrawal pw:
                        if (pw.Punter.SellerId != sellerId.Value)
                            throw new UnauthorizedAccessException("Esse saque não pertence a você.");
                        break;

                    case SellerWithdrawal:
                        // seller NUNCA pode autorizar saque de seller
                        throw new UnauthorizedAccessException("Sellers não podem concluir saques de sellers.");
                }
            }


            if (withdrawal is PunterWithdrawal punterWithdrawal)
            {
                var punter = punterWithdrawal.Punter;

                if (punter.PrizeBalance < withdrawal.Amount)
                    throw new InvalidOperationException("Saldo insuficiente para concluir o saque.");

                // Debita saldo
                var previousBalance = punter.PrizeBalance;
                punter.PrizeBalance -= withdrawal.Amount;

                // Registro histórico
                await _context.TransactionHistories.AddAsync(new TransactionHistory
                {
                    EntityType = "Punter",
                    EntityId = punter.Id,
                    PreviousBalance = previousBalance,
                    CurrentBalance = punter.PrizeBalance,
                    Amount = withdrawal.Amount,
                    Type = TransactionType.Withdrawal,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else if (withdrawal is SellerWithdrawal sellerWithdrawal)
            {
                var seller = sellerWithdrawal.Seller;

                // Verifica saldo do seller
                if (seller.Balance < withdrawal.Amount)
                    throw new InvalidOperationException("Saldo insuficiente para concluir o saque.");

                // Debita saldo
                var previousBalance = seller.Balance;
                seller.Balance -= withdrawal.Amount;

                // Registro histórico
                await _context.TransactionHistories.AddAsync(new TransactionHistory
                {
                    EntityType = "Seller",
                    EntityId = seller.Id,
                    PreviousBalance = previousBalance,
                    CurrentBalance = seller.Balance,
                    Amount = withdrawal.Amount,
                    Type = TransactionType.Withdrawal,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                throw new InvalidOperationException("Tipo de saque desconhecido.");
            }

            withdrawal.Status = EPaymentStatus.SUCCESS;
            withdrawal.ConfirmedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            transaction.Complete();
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[UpdateStatusToCompleted] ERRO: {ex.Message}");
            return false;
        }
    }

}
