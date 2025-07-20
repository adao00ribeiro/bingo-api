namespace bingo_api.src.DTOs.Shared;

public record EntityResponseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public EntityResponseDto(Guid id, DateTime CreatedAt, DateTime UpdatedAt)
    {
        Id = id;
        CreatedAt = CreatedAt;
        UpdatedAt = UpdatedAt;
    }

}
