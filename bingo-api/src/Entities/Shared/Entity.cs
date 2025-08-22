namespace bingo_api.src.Entities.Shared;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    public DateTime? DiscardedAt { get; set; } = null;
    protected Entity()
    {
        
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
