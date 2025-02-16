namespace bingo_api.src.Entities.Shared;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    protected Entity()
    {
        DateTime localDate = DateTime.Now; // Data no horário local
        DateTime utcDate = TimeZoneInfo.ConvertTimeToUtc(localDate); // Converte para UTC
        CreateAt = utcDate;
        UpdateAt = utcDate;
    }

    public void SetIdGuid(Guid guid)
    {

        this.Id = guid;
    }
}
