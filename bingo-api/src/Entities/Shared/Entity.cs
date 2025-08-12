namespace bingo_api.src.Entities.Shared;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DiscardedAt { get; set; } = null;
    protected Entity()
    {
        DateTime localDate = DateTime.Now; // Data no horário local
        DateTime utcDate = TimeZoneInfo.ConvertTimeToUtc(localDate); // Converte para UTC
        CreatedAt = utcDate;
        UpdatedAt = utcDate;
    }

    public void SetIdGuid(Guid guid)
    {
        this.Id = guid;
    }
    public void Discard()
    {
        DiscardedAt = DateTime.UtcNow;
    }
    public bool IsDiscarded()
    {
        return DiscardedAt != null;
    }
}
