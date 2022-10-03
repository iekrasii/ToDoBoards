using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ToDoBoards.Api.Features.RequestRateLimit.Interfaces;

namespace ToDoBoards.Api.Features.RequestRateLimit;

/// <summary>
/// Rate Limit Middleware
/// </summary>
public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IRateLimitService rateLimitService)
    {
        if (EndpointIsUnlimited(context))
        {
            await _next(context);
            return;
        }

        if (await rateLimitService.LimitIsReachedAsync(context.Request.Path, CancellationToken.None))
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            return;
        }

        await _next(context);
    }

    private bool EndpointIsUnlimited(HttpContext context)
    {
        var isLimitedAttribute = context.GetEndpoint()?.Metadata.GetMetadata<IsLimitedAttribute>();
        return isLimitedAttribute == null;
    }
}