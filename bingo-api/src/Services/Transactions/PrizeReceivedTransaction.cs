using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services.Transactions;

public class PrizeReceivedTransaction : ITransactionStrategy
{
    public void Execute(ITransactionParticipant participant, decimal amount)
    {
        participant.PrizeBalance += amount;
    }
}
