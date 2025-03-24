namespace bingo_api.src.DTOs.Shared;

public record EntityResponseDto
{
    public Guid Id { get;  set; } 
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; } 
}
