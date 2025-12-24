using System.Text.Json.Serialization;
using bingo_api.src.Entities;
using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response;

public record RoundResponseDto
{
    public Guid Id { get; set; }
    public decimal CardValue { get; set; }
    public int[] Numbers { get; set; } = [];
    public int TimeBetweenBalls { get; set; }
    public int MaxBalls { get; set; }
    public int CardRows { get; set; }
    public int CardColumns { get; set; }
    public int CardsPurchased { get; set; }

    // 🔥 EXTRA ATTRIBUTE
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, TimelineEvent> Timeline { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Finished { get; set; }
    public Guid RoomId { get; set; }
    public RoomResponseDto? Room { get; set; }

    [JsonIgnore]
    public IEnumerable<CardResponseDto>? Cards { get; set; }

    public IEnumerable<PrizeResponseDto>? Prizes { get; set; }

    public RoundResponseDto(
        Guid id,
        decimal cardValue,
        int[] numbers,
        int timeBetweenBalls,
        int maxBalls,
        int cardRows,
        int cardColumns,
        int cardsPurchased,
        DateTime started,
        DateTime? finished,
        Guid roomId,
        RoomResponseDto? room,
        IEnumerable<CardResponseDto>? cards,
        IEnumerable<PrizeResponseDto>? prizes,
        Dictionary<string, TimelineEvent>? timeline = null // 👈 EXTRA
    )
    {
        Id = id;
        CardValue = cardValue;
        Numbers = numbers;
        TimeBetweenBalls = timeBetweenBalls;
        MaxBalls = maxBalls;
        CardRows = cardRows;
        CardColumns = cardColumns;
        CardsPurchased = cardsPurchased;
        Started = started;
        Finished = finished;
        RoomId = roomId;
        Room = room;
        Cards = cards;
        Prizes = prizes;
        Timeline = timeline;
    }

    // ======================================================
    // NORMAL DTO (sem timeline)
    // ======================================================
    internal static RoundResponseDto ConvertToDto(Round round)
    {
        return new RoundResponseDto(
            round.Id,
            round.CardValue,
            round.Numbers,
            round.TimeBetweenBalls,
            round.MaxBalls,
            round.CardRows,
            round.CardColumns,
            round.CardsPurchased,
            round.Started,
            round.Finished,
            round.RoomId,
            round.Room != null ? RoomResponseDto.ConvertToDto(round.Room) : null,
            null,
            round.Prizes?.Select(PrizeResponseDto.ConvertToDto)
        );
    }

    // ======================================================
    // SOCKET DTO (sem room, sem cards)
    // ======================================================
    internal static RoundResponseDto ConvertToSocketDto(Round round)
    {
        return new RoundResponseDto(
            round.Id,
            round.CardValue,
            round.Numbers,
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

    // ======================================================
    // DTO COM EXTRA FIELDS (timeline)
    // ======================================================
    internal static RoundResponseDto ConvertToDtoWithTimeline(Round round)
    {
        return new RoundResponseDto(
            round.Id,
            round.CardValue,
            round.Numbers,
            round.TimeBetweenBalls,
            round.MaxBalls,
            round.CardRows,
            round.CardColumns,
            round.CardsPurchased,
            round.Started,
            round.Finished,
            round.RoomId,
            round.Room != null ? RoomResponseDto.ConvertToDto(round.Room) : null,
            null,
            round.Prizes?.Select(PrizeResponseDto.ConvertToDto),
            round.Timeline // 🔥 aqui entra o extra attribute
        );
    }
}
