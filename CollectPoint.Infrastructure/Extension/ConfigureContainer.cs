using Microsoft.AspNetCore.Builder;
using CollectPoint.Infrastructure.Middleware;

namespace CollectPoint.Infrastructure.Extension;

public static class ConfigureContainer
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<CustomExceptionMiddleware>();
}
