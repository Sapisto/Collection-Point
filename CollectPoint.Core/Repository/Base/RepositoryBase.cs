using Microsoft.Extensions.DependencyInjection;
using CollectPoint.Persistence;

namespace CollectPoint.Core.Repository.Base;

public abstract class RepositoryBase
{
    protected readonly IServiceScopeFactory _serviceScopeFactory;

    protected RepositoryBase(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected static AppDbContext GetDatabaseContext(IServiceScope scope)
        => scope.ServiceProvider.GetRequiredService<AppDbContext>();
}
