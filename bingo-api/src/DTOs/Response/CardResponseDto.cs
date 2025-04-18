using System.Text.Json.Serialization;
using bingo_api.src.Entities;


namespace bingo_api.src.DTOs.Response;

public record CardResponseDto
{
    public Guid Id { get; set; }
    public int[] Numbers { get; set; }
    public Guid RoundId { get; set; }
    public RoundResponseDto? Round { get; set; }
    public Guid PunterId { get; set; }
    public PunterResponseDto? Punter { get; set; }
     [JsonIgnore]
    public IEnumerable<CardWinnerResponseDto> CardWinners { get; set; }

    public CardResponseDto(Guid id, int[] numbers, Guid roundId, RoundResponseDto? round, Guid punterId, PunterResponseDto? punter, IEnumerable<CardWinnerResponseDto> cardWinners)
    {
        Id = id;
        Numbers = numbers;
        RoundId = roundId;
        Round = round;
        PunterId = punterId;
        Punter = punter;
        CardWinners = cardWinners;
    }
    internal static CardResponseDto ConvertToDto(Card card)
    {

        var RoundResponse = card.Round != null ? RoundResponseDto.ConvertToDto(card.Round) : null;
        var PunterResponse = card.Punter != null ? PunterResponseDto.ConvertToDto(card.Punter) : null;
        var CardsWinnersResponse = card.CardWinners?.Select(c => CardWinnerResponseDto.ConvertToDto(c)) ?? Enumerable.Empty<CardWinnerResponseDto>();

        return new CardResponseDto(
        card.Id,
        card.Numbers,
        card.RoundId,
        RoundResponse,
        card.PunterId,
        PunterResponse,
        CardsWinnersResponse
        );
    }
     public static CardResponseDto ConvertToSocketDto(Card card)
    {
        var RoundResponse = card.Round != null ? RoundResponseDto.ConvertToSocketDto(card.Round) : null;
        return new CardResponseDto(
        card.Id,
        card.Numbers,
        card.RoundId,
        RoundResponse,
        card.PunterId,
        null,
        null
        );
    }
}
