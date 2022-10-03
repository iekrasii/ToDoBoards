using System;
using System.Threading;
using System.Threading.Tasks;
using ToDoBoards.Api.Features.RequestRateLimit.Configuration;
using ToDoBoards.Api.Features.RequestRateLimit.Interfaces;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Api.Features.RequestRateLimit.Services;

/// <inheritdoc />
public class RateLimitService : IRateLimitService
{
    private readonly IRateLimitStorage _rateLimitStorage;
    private readonly RateLimitConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitService"/> class
    /// </summary>
    /// <param name="rateLimitStorage">Storage that holds RateLimit entities</param>
    /// <param name="configuration">Configuration object</param>
    public RateLimitService(IRateLimitStorage rateLimitStorage, RateLimitConfiguration configuration)
    {
        _rateLimitStorage = rateLimitStorage;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<bool> LimitIsReachedAsync(string apiPath, CancellationToken cancellationToken)
    {
        var rateLimit = await _rateLimitStorage.GetRateLimitAsync(apiPath, cancellationToken);
        if (rateLimit == null)
        {
            return await ProcessFirstApiRequestAsync(apiPath, cancellationToken);
        }

        if (rateLimit.CountApiCalls < _configuration.LimitApiRequestsCount)
        {
            return await ProcessLimitIsNotReached(rateLimit, cancellationToken);
        }

        var lastDateInTimeWindow = DateTime.UtcNow.AddDays(-_configuration.LimitPeriodDays).Date;
        if (rateLimit.Updated.Date <= lastDateInTimeWindow)
        {
            return await ProcessRateLimitOutdated(rateLimit, cancellationToken);
        }

        return true;
    }

    private async Task<bool> ProcessFirstApiRequestAsync(string apiPath, CancellationToken cancellationToken)
    {
        if (_configuration.LimitApiRequestsCount == 0)
            return true;

        var rateLimit = new RateLimit
        {
            ApiPath = apiPath,
            CountApiCalls = 1
        };

        await _rateLimitStorage.CreateOrUpdateRateLimitAsync(rateLimit, cancellationToken);
        return false;
    }

    private async Task<bool> ProcessLimitIsNotReached(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        rateLimit.CountApiCalls++;
        await _rateLimitStorage.CreateOrUpdateRateLimitAsync(rateLimit, cancellationToken);
        return false;
    }

    private async Task<bool> ProcessRateLimitOutdated(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        rateLimit.CountApiCalls = 0;
        await _rateLimitStorage.CreateOrUpdateRateLimitAsync(rateLimit, cancellationToken);
        return false;
    }
}