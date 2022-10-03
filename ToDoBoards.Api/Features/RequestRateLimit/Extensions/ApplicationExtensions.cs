using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using ToDoBoards.Api.Features.RequestRateLimit.Configuration;
using ToDoBoards.Common.Extensions;

namespace ToDoBoards.Api.Features.RequestRateLimit.Extensions;

public static class ApplicationExtensions
{
    /// <summary>
    /// Applies Rate Limit Middleware
    /// </summary>
    /// <param name="app">Application</param>
    /// <param name="configuration">Configuration</param>
    public static void UseRequestRateLimit(this WebApplication app, IConfiguration configuration)
    {
        var settings = configuration.BindSettings<RateLimitConfiguration>();

        if (settings.Enabled)
        {
            app.UseMiddleware<RateLimitMiddleware>();
        }
    }
}