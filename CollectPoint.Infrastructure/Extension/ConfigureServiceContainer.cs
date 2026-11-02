using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CollectPoint.Core.Contract;
using CollectPoint.Core.Contract.Repository;
using CollectPoint.Core.Implementation;
using CollectPoint.Core.Repository;
using CollectPoint.Domain.Common;
using CollectPoint.Persistence;

namespace CollectPoint.Infrastructure.Extension;

public static class ConfigureServiceContainer
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
    }

    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
    }

    public static void AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
            apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
            {
                var logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<BadRequestObjectResult>>();
                IEnumerable<string> errorList = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                logger.LogError("Validation failed: {Errors}", string.Join(", ", errorList));
                return new BadRequestObjectResult(new Response<IEnumerable<string>>("Validation Error", 400, errorList));
            });
    }

    public static void AddSwaggerOpenAPI(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
