using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CollectPoint.Core.Exceptions;
using CollectPoint.Domain.Common;

namespace CollectPoint.Infrastructure.Middleware;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);   // like next() in Express
        }
        catch (ApiException ex)
        {
            await WriteAsync(context, (int)ex.HttpStatusCode, new Response<string>(ex.Message, ex.ErrorCode));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            // Don't send ex.Message to the client for unexpected errors; it can leak DB details
            await WriteAsync(context, StatusCodes.Status500InternalServerError, new Response<string>("Something went wrong.", 500));
        }
    }

    private static Task WriteAsync(HttpContext context, int statusCode, Response<string> body)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(body);
    }
}
