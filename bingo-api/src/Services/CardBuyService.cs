using System.Transactions;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Interfaces.Services;

namespace bingo_api.src.Services;

public class CardBuyService : ICardBuyService
{
    private readonly ICardRepository _cardRepository;
    private readonly IPunterRepository _punterRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly ICardBuyRepository _cardBuyRepository;
    private readonly ITransactionHistoryRepository transactionHistoryRepository;

    public CardBuyService(
        ICardRepository cardRepository,
        IPunterRepository punterRepositoryy,
         IRoundRepository roundRepository,
         ICardBuyRepository cardBuyRepository,
         ITransactionHistoryRepository _transactionHistoryRepository
         )
    {
        this._cardRepository = cardRepository;
        this._punterRepository = punterRepositoryy;
        this._roundRepository = roundRepository;
        this._cardBuyRepository = cardBuyRepository;
        this.transactionHistoryRepository = _transactionHistoryRepository;
    }
    public async Task<bool> Buy(CardBuyRequestDto dto)
    {

        if (dto == null || dto.PunterId == Guid.Empty || dto.Quantity == 0)
        {
            throw new Exception("Nao encontrado");
        }

        Punter? punter = await this._punterRepository.GetByIdAsync(dto.PunterId);

        if (punter is null)
        {
            throw new Exception("pUNTER Nao encontrado");
        }
        Round? round = await this._roundRepository.GetByIdAsync(dto.RoundId);

        if (round is null)
        {
            throw new Exception("round Nao encontrado");
        }
        if (round.Started < DateTime.UtcNow)
        {
            throw new Exception("The round is already past the scheduled time.");
        }

        var value = round.CardValue * dto.Quantity;
        if (punter.Balance < value)
        {
            throw new Exception("Saldo insuficiente. Por favor, recarregue sua conta para continuar.");
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var cardbuy = CardBuyRequestDto.ConvertToEntity(dto);

                var cardBuyId = await this._cardBuyRepository.AddAsync(cardbuy);


                if (cardBuyId == Guid.Empty)
                {
                    throw new Exception("Compra nao realizada");
                }


                var cardsToInsert = new List<Card>();

                for (int i = 0; i < dto.Quantity; i++)
                {
                    var card = new Card
                    {
                        Numbers = GetRandomNumbers(round.MaxBalls, round.CardRows, round.CardColumns),
                        Name = punter.Name,
                        Code = new Random().Next(1, 100000),
                        RoundId = round.Id,
                        PunterId = punter.Id,
                        CardBuyId = cardBuyId
                    };
                    cardsToInsert.Add(card);
                }

                await this._cardRepository.AddRangeAsync(cardsToInsert);
                // Registra a transação no histórico
                var transactionHistory = new TransactionHistory
                {
                    EntityType = "Punter", // Pode ser Seller se o participante for um Seller
                    EntityId = punter.Id,
                    PreviousBalance = punter.Balance, // Antes da alteração
                    CurrentBalance = punter.Balance - value, // O saldo será alterado após o registro da transação
                    Amount = -value,
                    Type = TransactionType.CardPurchased, // Assume que Purchase é o tipo de transação para compra de cartela
                };

                // Registra o histórico da transação antes de alterar o saldo
                await this.transactionHistoryRepository.AddAsync(transactionHistory);

                round.CardSaleCount += dto.Quantity;
                punter.Balance -= value;
                await this._roundRepository.UpdateAsync(round);
                await this._punterRepository.UpdateAsync(punter);
                transaction.Complete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public static int[] GetRandomNumbers(int maxNumber, int linha, int coluna)
    {
        List<int> result = new List<int>();
        Random random = new Random();
        List<int> availableNumbers = Enumerable.Range(1, maxNumber).ToList();

        for (int i = 0; i < linha; i++)
        {
            HashSet<int> uniqueRow = new HashSet<int>();

            // Gera um conjunto de números únicos para a linha
            while (uniqueRow.Count < coluna)
            {
                int randomIndex = random.Next(availableNumbers.Count);
                int selectedNumber = availableNumbers[randomIndex];

                uniqueRow.Add(selectedNumber);
                availableNumbers.RemoveAt(randomIndex);
            }

            // Ordena a linha e adiciona ao resultado final
            var sortedRow = uniqueRow.OrderBy(num => num).ToArray();
            result.AddRange(sortedRow);
        }

        return result.ToArray();
    }
}
