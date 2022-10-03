using System.Threading;
using System.Threading.Tasks;
using ToDoBoards.Api.Features.RequestRateLimit.Configuration;
using ToDoBoards.Api.Features.RequestRateLimit.Interfaces;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Models.Notifications;

namespace ToDoBoards.Api.Features.RequestRateLimit.Services;

/// <summary>
/// This is decorator over <see cref="RateLimitService"/> class.
/// It is aimed to send notification to admin when appropriate.  
/// </summary>
public class NotificationDecorator : IRateLimitService
{
    private readonly RateLimitService _rateLimitService;
    private readonly IRateLimitStorage _rateLimitStorage;
    private readonly INotifier _notifier;
    private readonly RateLimitConfiguration _configuration;

    public NotificationDecorator(RateLimitService rateLimitService, IRateLimitStorage rateLimitStorage, INotifier notifier, RateLimitConfiguration configuration)
    {
        _rateLimitService = rateLimitService;
        _rateLimitStorage = rateLimitStorage;
        _notifier = notifier;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<bool> LimitIsReachedAsync(string apiPath, CancellationToken cancellationToken)
    {
        var limitIsReached = await _rateLimitService.LimitIsReachedAsync(apiPath, cancellationToken);

        if (limitIsReached)
            return true;

        var rateLimit = await _rateLimitStorage.GetRateLimitAsync(apiPath, cancellationToken);

        if (rateLimit.NotificationIsSent)
            return false;

        if (IsTimeToSendNotification(rateLimit))
            await SendNotification(apiPath, rateLimit, cancellationToken);

        return false;
    }

    private bool IsTimeToSendNotification(RateLimit rateLimit)
    {
        double callsCountAfterWhichToNotify = _configuration.Notification.LimitPercentageForNotification * _configuration.LimitApiRequestsCount / 100.00;
        return rateLimit.CountApiCalls >= callsCountAfterWhichToNotify;
    }

    private Task SendNotification(string apiPath, RateLimit rateLimit, CancellationToken cancellationToken)
    {
        _notifier.Notify(new RateLimitAdminWarning
        {
            ApiPath = apiPath,
            PercentageReached = _configuration.Notification.LimitPercentageForNotification
        });

        rateLimit.NotificationIsSent = true;
        return _rateLimitStorage.CreateOrUpdateRateLimitAsync(rateLimit, cancellationToken);
    }
}