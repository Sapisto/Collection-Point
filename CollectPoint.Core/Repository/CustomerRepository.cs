using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CollectPoint.Core.Contract.Repository;
using CollectPoint.Core.Repository.Base;
using CollectPoint.Domain.Entities;
using CollectPoint.Domain.QueryParameters;

namespace CollectPoint.Core.Repository;

public class CustomerRepository : RepositoryBase, ICustomerRepository
{
    public CustomerRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }

    public async Task<(List<Customer> Items, int Total)> GetCustomersAsync(CustomerQueryParameters query, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = GetDatabaseContext(scope);

        // IQueryable: nothing hits the database until CountAsync/ToListAsync
        var customers = dbContext.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Query))
        {
            // ILIKE = case-insensitive search in Postgres ("ali" matches "Alhassan")
            string search = $"%{query.Query.Trim()}%";
            customers = customers.Where(x => EF.Functions.ILike(x.CustomerName, search) || EF.Functions.ILike(x.AccountNumber, search));
        }

        if (!string.IsNullOrWhiteSpace(query.Currency))
        {
            string currency = query.Currency.ToUpper();
            customers = customers.Where(x => x.Currency == currency);
        }

        int total = await customers.CountAsync(cancellationToken);

        var items = await customers
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<Customer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = GetDatabaseContext(scope);

        return await dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> AccountNumberExistsAsync(string accountNumber, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = GetDatabaseContext(scope);

        return await dbContext.Customers.AnyAsync(x => x.AccountNumber == accountNumber, cancellationToken);
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = GetDatabaseContext(scope);

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return customer;
    }
}
