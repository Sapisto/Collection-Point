using CollectPoint.Domain.Entities;

namespace CollectPoint.Core.DTO.Response;

public class CustomerResponse
{
    public string Id { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Manual mapping; FIRSPortal uses AutoMapper, which needs a paid licence since v15
    public static CustomerResponse From(Customer customer) => new()
    {
        Id = customer.Id,
        CustomerName = customer.CustomerName,
        AccountNumber = customer.AccountNumber,
        Currency = customer.Currency,
        CreatedAt = customer.CreatedAt
    };
}
