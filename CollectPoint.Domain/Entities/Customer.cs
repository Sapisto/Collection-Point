using CollectPoint.Domain.Entities.Base;

namespace CollectPoint.Domain.Entities;

public class Customer : EntityBase<string>
{
    public Customer()
    {
        SetNewId();
    }

    public string CustomerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public override void SetNewId() => Id = $"CUS_{Ulid.NewUlid()}";
}
