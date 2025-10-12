using bingo_api.src.DTOs.Shared;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record CardWinnerResponseDto : EntityResponseDto
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public Guid CardId { get; set; }
    public CardResponseDto? Card { get; set; }
    public Guid PrizeId { get; set; }
    public PrizeResponseDto? Prize { get; set; }

    public CardWinnerResponseDto(Guid id, decimal value, Guid cardId, CardResponseDto? card, Guid prizeId, DateTime CreatedAt,
        DateTime UpdatedAt, PrizeResponseDto? prize) : base(id, CreatedAt, UpdatedAt)
    {
        Id = id;
        Value = value;
        CardId = cardId;
        Card = card;
        PrizeId = prizeId;
        CreatedAt = CreatedAt;
        UpdatedAt = UpdatedAt;
        Prize = prize;
    }
    internal static CardWinnerResponseDto ConvertToDto(CardWinner cardWinner)
    {
        var cardResponse = cardWinner.Card != null ? CardResponseDto.ConvertToDto(cardWinner.Card) : null;
        var prizeResponse = cardWinner.Prize != null ? PrizeResponseDto.ConvertToDto(cardWinner.Prize) : null;
        return new CardWinnerResponseDto(
        cardWinner.Id,
        cardWinner.Value,
        cardWinner.CardId,
        cardResponse,
        cardWinner.PrizeId,
          cardWinner.CreatedAt,
            cardWinner.UpdatedAt,
        prizeResponse
     );
    }
}
