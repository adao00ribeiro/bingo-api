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
                Status = EWithdrawalStatus.PENDING
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
                Status = EWithdrawalStatus.PENDING
            };

            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            return (true, "Saque do Seller registrado com sucesso.");
        }

        return (false, "Entidade não encontrada ou tipo inválido.");
    }
}
