
using System.Text.Json.Serialization;
using bingo_api.src.DTOs.Request;
using bingo_api.src.Entities;

namespace bingo_api.src.DTOs.Response;

public record RoundResponseDto
{
    public Guid Id { get; set; }
    public decimal CardValue { get; set; }
    public int[] Numbers { get; set; }
    public int CardSaleCount { get; set; }
    public int TimeBetweenBalls { get; set; }
    public int MaxBalls { get; set; }//utilizado para jogos de 90 ,80,75, 50 ,30
    public int CardRows { get; set; } // Número de linhas na cartela
    public int CardColumns { get; set; } // Número de colunas na cartela

    public int CardsPurchased { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public Guid RoomId { get; set; }
    public RoomResponseDto? Room { get; set; }

    [JsonIgnore]
    public IEnumerable<CardResponseDto>? Cards { get; set; }
    public IEnumerable<PrizeResponseDto>? Prizes { get; set; }
    public RoundResponseDto(

        Guid id, decimal cardValue,

        int[] numbers,
        int cardSaleCount,
        int timeBetweenBalls,
        int maxBalls,
        int cardRows,
        int cardColumns,
        int cardsPurchased,
        DateTime startedDate,
        DateTime? finishedDate,
        Guid roomId,
        RoomResponseDto? room,
        IEnumerable<CardResponseDto> cards,
        IEnumerable<PrizeResponseDto> prizes)
    {
        Id = id;
        CardValue = cardValue;
        Numbers = numbers;
        CardSaleCount = cardSaleCount;
        TimeBetweenBalls = timeBetweenBalls;
        MaxBalls = maxBalls;
        CardRows = cardRows;
        CardColumns = cardColumns;
        CardsPurchased = cardsPurchased;
        StartedDate = startedDate;
        FinishedDate = finishedDate;
        RoomId = roomId;
        Room = room;
        Cards = cards;
        Prizes = prizes;
    }

    internal static RoundResponseDto ConvertToDto(Round round)
    {
        var roomResponse = round.Room != null ? RoomResponseDto.ConvertToDto(round.Room) : null;

        var prizesResponse = round.Prizes?.Select(x => PrizeResponseDto.ConvertToDto(x)) ?? Enumerable.Empty<PrizeResponseDto>();
        return new RoundResponseDto(
                round.Id,
                round.CardValue,
                round.Numbers,
                round.CardSaleCount,
                round.TimeBetweenBalls,
                round.MaxBalls,
                round.CardRows,
                round.CardColumns,
                round.CardsPurchased,
                round.Started,
                round.Finished,
                round.RoomId,
                roomResponse,
                null,
                prizesResponse

        );
    }

    internal static RoundResponseDto ConvertToSocketDto(Round round)
    {

        return new RoundResponseDto(
                round.Id,
                round.CardValue,
                round.Numbers,
                round.CardSaleCount,
                round.TimeBetweenBalls,
                round.MaxBalls,
                round.CardRows,
                round.CardColumns,
                round.CardsPurchased,
                round.Started,
                round.Finished,
                round.RoomId,
                null,
                null,
                 Enumerable.Empty<PrizeResponseDto>()

        );
    }
}
