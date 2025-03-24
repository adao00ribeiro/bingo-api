using bingo_api.src.Interfaces.Services;
using bingo_api.src.Services.Transactions;

namespace bingo_api.src.Factory;

public class TransactionFactory
{
    public static ITransactionStrategy Create(TransactionType type)
    {
        return type switch
        {
            TransactionType.Deposit => new DepositTransaction(),
            TransactionType.CardPurchased => new PurchaseTransaction(),
            TransactionType.PrizeReceived => new PrizeReceivedTransaction(),
            TransactionType.Withdrawal => new PrizeWithdrawTransaction(),
            _ => throw new ArgumentException("Tipo de transação inválido.")
        };
    }
}
