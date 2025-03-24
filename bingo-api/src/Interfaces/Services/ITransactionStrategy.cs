
namespace bingo_api.src.Interfaces.Services;

public interface ITransactionStrategy
{
      void Execute(ITransactionParticipant participant, decimal amount);
}
