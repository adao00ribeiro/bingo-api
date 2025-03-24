using System.Text.Json.Serialization;
using bingo_api.src.Entities;
using bingo_api.src.Enums;


namespace bingo_api.src.DTOs.Response;

public record PrizeResponseDto
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public EPrizeType Type { get; set; }
    public Guid RoundId { get; set; }
    [JsonIgnore]
    public RoundResponseDto? Round { get; set; }
    [JsonIgnore]
    public IEnumerable<CardWinnerResponseDto>? CardWinners { get; set; }
    public PrizeResponseDto(Guid id, decimal value, EPrizeType prizeType, Guid roundId, RoundResponseDto? round, IEnumerable<CardWinnerResponseDto> cardwinners)
    {
        Id = id;
        Value = value;
        Type = prizeType;
        RoundId = roundId;
        Round = round;
        CardWinners = cardwinners;
    }
    internal static PrizeResponseDto ConvertToDto(Prize prize)
    {

        var roundResponseDto = prize.Round != null ? RoundResponseDto.ConvertToDto(prize.Round) : null;
        var cardWinnerResponse = prize.CardWinners?.Select(x => CardWinnerResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<CardWinnerResponseDto>();
        return new PrizeResponseDto(
            prize.Id,
                prize.Value,
                prize.Type,
                prize.RoundId,
                roundResponseDto,
                cardWinnerResponse
        );
    }

    internal static PrizeResponseDto ConvertToSocketDto(Prize prize)
    {
        return new PrizeResponseDto(
                prize.Id,
                prize.Value,
                prize.Type,
                prize.RoundId,
                null,
                Enumerable.Empty<CardWinnerResponseDto>()
        );
    }
}
