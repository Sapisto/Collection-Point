using CollectPoint.Domain.Entities;
using CollectPoint.Domain.QueryParameters;

namespace CollectPoint.Core.Contract.Repository;

public interface ICustomerRepository
{
    Task<(List<Customer> Items, int Total)> GetCustomersAsync(CustomerQueryParameters query, CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<bool> AccountNumberExistsAsync(string accountNumber, CancellationToken cancellationToken);
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);
}
