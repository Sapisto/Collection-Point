namespace CollectPoint.Domain.Entities.Base;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public abstract void SetNewId();
}
