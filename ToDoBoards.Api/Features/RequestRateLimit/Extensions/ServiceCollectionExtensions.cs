using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoBoards.Api.Features.RequestRateLimit.Configuration;
using ToDoBoards.Api.Features.RequestRateLimit.Interfaces;
using ToDoBoards.Api.Features.RequestRateLimit.Services;
using ToDoBoards.Common.Extensions;
using ToDoBoards.Notification;

namespace ToDoBoards.Api.Features.RequestRateLimit.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers rate limit feature services
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRequestRateLimit(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = configuration.BindSettings<RateLimitConfiguration>();

        if (settings.Enabled)
        {
            serviceCollection.AddScoped(sp => settings);
            serviceCollection.AddScoped<IRateLimitService, RateLimitService>();
            
            if (settings.Notification.Enabled)
            {
                serviceCollection.AddScoped<RateLimitService>();
                serviceCollection.AddScoped<IRateLimitService, NotificationDecorator>();
                // TODO: to be replaced with KafkaNotifier
                serviceCollection.AddConsoleNotifier();
            }
        }

        return serviceCollection;
    }
}