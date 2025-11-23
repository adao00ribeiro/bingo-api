using bingo_api.src.Interfaces;

namespace bingo_api.src.Entities.Shared;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
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
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

}
