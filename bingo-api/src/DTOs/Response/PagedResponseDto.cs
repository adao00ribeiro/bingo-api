namespace bingo_api.src.DTOs.Response;

public record PagedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
