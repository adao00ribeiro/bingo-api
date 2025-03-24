using System.ComponentModel.DataAnnotations.Schema;
using bingo_api.src.DTOs.Response;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Enums;
using bingo_api.src.Structs;

namespace bingo_api.src.Entities;

public class Prize : Entity
{
    public decimal Value { get; set; }
    public EPrizeType Type { get; set; }
    public Guid RoundId { get; set; }
    public Round? Round { get; set; }
    public IEnumerable<CardWinner>? CardWinners { get; set; }
    public Prize(decimal value)
    {
        this.Value = value;
    }
    public Prize(decimal value, EPrizeType type, Guid roundId)
    {
        this.Value = value;
        this.Type = type;
        this.RoundId = roundId;
    }
    public Prize(decimal value, EPrizeType type)
    {
        this.Value = value;
        this.Type = type;
    }
    // daqui para baixo e so usado para logica de negocio
    [NotMapped]
    public bool RefreshWinner { get; private set; } = false;

    [NotMapped]
    public List<TopCardInfo> TopCards { get; private set; } = new List<TopCardInfo>();

    [NotMapped]
    public List<WinningCardsInfo> WinningCards { get; private set; } = new List<WinningCardsInfo>();
    public void SetRefresWinner(bool IsActive)
    {
        RefreshWinner = IsActive;
    }
    public bool HasWinners() => WinningCards.Any();
    internal PrizeResult GetObject()
    {
        return new PrizeResult
        {
            PrizeType = Type,
            WinningCards = WinningCards,
            ListTopCards = TopCards
        };
    }

    public void SetTopFive(Card card, int hits, List<int> missingNumbers)
    {
        var newCardInfo = new TopCardInfo
        {
            Card =CardResponseDto.ConvertToSocketDto(card),
            Punter =  PunterResponseDto.ConvertToSocketDto(card.Punter),
            MissingNumbers = missingNumbers,
            Hits = hits
        };

        // Verifica se já existe um cartão com o mesmo ID
        var existingCard = TopCards.FirstOrDefault(obj => obj.Card.Id == card.Id);

        if (existingCard != null)
        {
            // Atualiza o `hits` e `missingNumbers` do cartão existente
            existingCard.Hits = hits;
            existingCard.MissingNumbers = missingNumbers;
        }
        else
        {
            // Adiciona o novo cartão
            TopCards.Add(newCardInfo);
        }
        // Ordena `ListTopCards` pelos maiores hits em ordem decrescente e limita aos top 20
        TopCards = TopCards
            .OrderByDescending(obj => obj.Hits)
            .ThenBy(obj => obj.MissingNumbers.Count) // Para desempate, prioriza menos números faltantes
            .Take(20)
            .ToList();
    }

    public void AddAccumulated(decimal accumulatedValue)
    {
        if (Type != EPrizeType.FullCard)
        {
            return;
        }

        decimal distributedValue = accumulatedValue / WinningCards.Count;

        foreach (var winner in WinningCards)
        {
            winner.ValueOfEachWinner = (decimal)winner.ValueOfEachWinner + distributedValue;
        }

        this.Value += distributedValue;
    }
}
